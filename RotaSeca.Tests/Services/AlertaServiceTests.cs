using Moq;
using RotaSeca.Domain.Entities;
using RotaSeca.Domain.ValueObjects;
using RotaSeca.Integrations;
using RotaSeca.Repositories;
using RotaSeca.Services;

namespace RotaSeca.Tests.Services;

public class AlertaServiceTests
{
    private readonly Mock<IAlertaRepository> _alertas = new();
    private readonly Mock<ILugarRepository> _lugares = new();
    private readonly Mock<IRiscoProvider> _risco = new();
    private readonly AlertaService _service;

    public AlertaServiceTests()
    {
        _service = new AlertaService(_alertas.Object, _lugares.Object, _risco.Object);

        _lugares.Setup(l => l.ListarPorUsuarioAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LugarSalvo>
                {
                    new(1, "Casa da vó", "Endereço simulado", new Coordenada(-23.545, -46.638))
                });
    }

    private void ConfigurarRisco(double valor) =>
        _risco.Setup(r => r.ObterProbabilidadeAsync(It.IsAny<Coordenada>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(valor);

    [Fact]
    public async Task RiscoAlto_GeraAlerta()
    {
        ConfigurarRisco(0.80);

        var gerados = await _service.GerarAlertasAsync(1);

        Assert.Equal(1, gerados);
        _alertas.Verify(a => a.AdicionarAsync(
            It.Is<Alerta>(x => x.Mensagem.Contains("Casa da vó")), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RiscoBaixo_NaoGeraAlerta()
    {
        ConfigurarRisco(0.30);

        Assert.Equal(0, await _service.GerarAlertasAsync(1));
        _alertas.Verify(a => a.AdicionarAsync(It.IsAny<Alerta>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AlertaRecenteExistente_NaoDuplica()
    {
        ConfigurarRisco(0.90);
        _alertas.Setup(a => a.ExisteAlertaRecenteAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

        Assert.Equal(0, await _service.GerarAlertasAsync(1));
    }
}