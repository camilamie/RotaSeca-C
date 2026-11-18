
using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.Modos;

namespace RotaSeca.Tests.Domain;

public class ModoDeslocamentoTests
{
    private readonly ModoDeslocamentoFactory _factory =
        new(new IModoDeslocamento[] { new APe(), new MotoBike(), new CarroOnibus() });

    [Theory]
    [InlineData(ModoDeslocamentoTipo.APe, 0.39, false)]
    [InlineData(ModoDeslocamentoTipo.APe, 0.40, true)]
    [InlineData(ModoDeslocamentoTipo.MotoBike, 0.45, false)]
    [InlineData(ModoDeslocamentoTipo.MotoBike, 0.50, true)]
    [InlineData(ModoDeslocamentoTipo.CarroOnibus, 0.65, false)]
    [InlineData(ModoDeslocamentoTipo.CarroOnibus, 0.70, true)]
    public void DeveBloquear_RespeitaOLimiteDeCadaModo(ModoDeslocamentoTipo tipo, double risco, bool esperado)
    {
        var modo = _factory.Obter(tipo);
        Assert.Equal(esperado, modo.DeveBloquear(risco));
    }

    [Fact]
    public void CarroOnibus_SomaTempoDeParadasNaDuracao()
    {
        // 11 km a 22 km/h = 30 min + 5 min de paradas
        Assert.Equal(35, new CarroOnibus().EstimarDuracaoMinutos(11));
    }

    [Fact]
    public void APe_CalculaDuracaoPelaVelocidadeDeCaminhada()
    {
        // 2,5 km a 5 km/h = 30 min
        Assert.Equal(30, new APe().EstimarDuracaoMinutos(2.5));
    }

    [Fact]
    public void Factory_ModoInexistente_LancaDomainException()
    {
        Assert.Throws<DomainException>(() => _factory.Obter((ModoDeslocamentoTipo)99));
    }
}