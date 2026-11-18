using Microsoft.Extensions.Logging.Abstractions;
using Moq;

using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.Modos;
using RotaSeca.Domain.ValueObjects;
using RotaSeca.DTOs;
using RotaSeca.Integrations;
using RotaSeca.Repositories;
using RotaSeca.Services;

namespace RotaSeca.Tests.Services;

public class RotaServiceTests
{
    private readonly Mock<IRotaRepository> _rotas = new();
    private readonly Mock<IUsuarioRepository> _usuarios = new();
    private readonly Mock<IRoteadorProvider> _roteador = new();
    private readonly Mock<IRiscoProvider> _risco = new();
    private readonly Mock<IChuvaProvider> _chuva = new();
    private readonly RotaService _service;

    public RotaServiceTests()
    {
        var modos = new ModoDeslocamentoFactory(new IModoDeslocamento[] { new APe(), new MotoBike(), new CarroOnibus() });

        _service = new RotaService(_rotas.Object, _usuarios.Object, _roteador.Object, _risco.Object,
                                   _chuva.Object, modos, NullLogger<RotaService>.Instance);

        _usuarios.Setup(u => u.ExisteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _chuva.Setup(c => c.ObterChuvaAsync(It.IsAny<Coordenada>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new ChuvaAtual(20, 24, DateTime.UtcNow, "Teste"));

        // Três rotas, cada uma com um ponto numa "faixa" de latitude diferente
        _roteador.Setup(r => r.CalcularRotasAsync(It.IsAny<Coordenada>(), It.IsAny<Coordenada>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<RotaCandidata>
                 {
                     new("Via direta", 2.0, new[] { new Coordenada(-23.50, -46.60) }),
                     new("Alternativa 1", 3.0, new[] { new Coordenada(-23.60, -46.60) }),
                     new("Alternativa 2", 4.0, new[] { new Coordenada(-23.70, -46.60) })
                 });
    }

    private void ConfigurarRiscos(double direta, double alternativa1, double alternativa2) =>
        _risco.Setup(r => r.ObterProbabilidadeAsync(It.IsAny<Coordenada>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync((Coordenada c, DateTime _, CancellationToken __) =>
                  c.Latitude > -23.55 ? direta : c.Latitude > -23.65 ? alternativa1 : alternativa2);

    private static CalcularRotaRequest Requisicao(ModoDeslocamentoTipo modo) =>
        new(1, new CoordenadaDto(-23.50, -46.60), new CoordenadaDto(-23.52, -46.62), modo);

    [Fact]
    public async Task APe_BloqueiaRotasAcimaDe40PorCento_ERecomendaAMaisSegura()
    {
        ConfigurarRiscos(direta: 0.90, alternativa1: 0.10, alternativa2: 0.45);

        var resultado = await _service.CalcularAsync(Requisicao(ModoDeslocamentoTipo.APe));

        Assert.False(resultado.SemCaminhoSeco);
        Assert.Equal(2, resultado.Opcoes.Count(o => o.Bloqueada));
        var recomendada = Assert.Single(resultado.Opcoes, o => o.Recomendada);
        Assert.Equal("Alternativa 1", recomendada.Descricao);
    }

    [Fact]
    public async Task CarroOnibus_ComMesmoRisco_BloqueiaMenosRotas()
    {
        ConfigurarRiscos(direta: 0.90, alternativa1: 0.10, alternativa2: 0.45);

        var resultado = await _service.CalcularAsync(Requisicao(ModoDeslocamentoTipo.CarroOnibus));

        Assert.Equal(1, resultado.Opcoes.Count(o => o.Bloqueada)); // só a direta (90% >= 70%)
    }

    [Fact]
    public async Task TodasAsRotasAlagadas_InformaQueNaoHaCaminhoSeco()
    {
        ConfigurarRiscos(0.95, 0.85, 0.80);

        var resultado = await _service.CalcularAsync(Requisicao(ModoDeslocamentoTipo.APe));

        Assert.True(resultado.SemCaminhoSeco);
        Assert.DoesNotContain(resultado.Opcoes, o => o.Recomendada);
        Assert.Contains("Nenhum caminho seco", resultado.Mensagem);
    }

    [Fact]
    public async Task CalcularAsync_GravaAConsultaNoHistorico()
    {
        ConfigurarRiscos(0.10, 0.10, 0.10);

        await _service.CalcularAsync(Requisicao(ModoDeslocamentoTipo.MotoBike));

        _rotas.Verify(r => r.AdicionarAsync(It.IsAny<Rota>(), It.IsAny<CancellationToken>()), Times.Once);
        _rotas.Verify(r => r.SalvarAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UsuarioInexistente_LancaNotFoundException()
    {
        var requisicao = Requisicao(ModoDeslocamentoTipo.APe) with { UsuarioId = 999 };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CalcularAsync(requisicao));
        _rotas.Verify(r => r.SalvarAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}