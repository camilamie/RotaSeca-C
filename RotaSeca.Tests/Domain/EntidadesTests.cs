
using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Tests.Domain;

public class EntidadesTests
{
    private static readonly Coordenada Centro = new(-23.5450, -46.6380);

    [Fact]
    public void LugarSalvo_EtiquetaVazia_LancaDomainException()
    {
        Assert.Throws<DomainException>(() => new LugarSalvo(1, "", "Endereço simulado", Centro));
    }

    [Fact]
    public void LugarSalvo_SemDiasCadastrados_FrequentaTodosOsDias()
    {
        var lugar = new LugarSalvo(1, "Casa", "Endereço simulado", Centro);
        Assert.True(lugar.FrequentaNoDia(DayOfWeek.Sunday));
    }

    [Fact]
    public void LugarSalvo_ComDiasUteis_NaoFrequentaNoSabado()
    {
        var lugar = new LugarSalvo(1, "Trabalho", "Endereço simulado", Centro, DiasSemana.DiasUteis);
        Assert.False(lugar.FrequentaNoDia(DayOfWeek.Saturday));
        Assert.True(lugar.FrequentaNoDia(DayOfWeek.Monday));
    }

    [Fact]
    public void Coordenada_LatitudeInvalida_LancaDomainException()
    {
        Assert.Throws<DomainException>(() => new Coordenada(-120, -46.6));
    }

    [Fact]
    public void Reporte_DuasConfirmacoes_FicaConfirmado()
    {
        var reporte = new Reporte(1, Centro, NivelAgua.Joelho);
        reporte.Confirmar();
        Assert.Equal(StatusReporte.Ativo, reporte.Status);

        reporte.Confirmar();
        Assert.Equal(StatusReporte.Confirmado, reporte.Status);
    }

    [Fact]
    public void Reporte_DoisAvisosDeAguaBaixou_EncerraEImpedeNovasConfirmacoes()
    {
        var reporte = new Reporte(1, Centro, NivelAgua.Canela);
        reporte.RegistrarQueAguaBaixou();
        reporte.RegistrarQueAguaBaixou();

        Assert.Equal(StatusReporte.Encerrado, reporte.Status);
        Assert.NotNull(reporte.EncerradoEm);
        Assert.Throws<DomainException>(() => reporte.Confirmar());
    }

    [Fact]
    public void Rota_OrigemIgualAoDestino_LancaDomainException()
    {
        Assert.Throws<DomainException>(() => new Rota(1, Centro, Centro, ModoDeslocamentoTipo.APe));
    }

    [Theory]
    [InlineData(0.55, NivelAlerta.Atencao)]
    [InlineData(0.70, NivelAlerta.Alto)]
    [InlineData(0.90, NivelAlerta.Critico)]
    public void Alerta_ClassificaNivelPelaProbabilidade(double probabilidade, NivelAlerta esperado)
    {
        Assert.Equal(esperado, Alerta.ClassificarNivel(probabilidade));
    }
}