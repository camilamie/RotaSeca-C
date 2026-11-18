using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Domain.Entities;

public class Reporte : EntidadeBase
{
    public const int ConfirmacoesNecessarias = 2;

    public int? UsuarioId { get; private set; } 
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public NivelAgua Nivel { get; private set; }
    public string? Comentario { get; private set; }
    public StatusReporte Status { get; private set; } = StatusReporte.Ativo;
    public int Confirmacoes { get; private set; }
    public int ConfirmacoesDeBaixa { get; private set; }
    public DateTime? EncerradoEm { get; private set; }

    protected Reporte() { }

    public Reporte(int? usuarioId, Coordenada local, NivelAgua nivel, string? comentario = null)
    {
        UsuarioId = usuarioId;
        Latitude = local.Latitude;
        Longitude = local.Longitude;
        Nivel = nivel;
        Comentario = string.IsNullOrWhiteSpace(comentario) ? null : comentario.Trim();
        Validar();
    }

    public void Confirmar()
    {
        GarantirQueNaoEstaEncerrado();
        Confirmacoes++;
        if (Confirmacoes >= ConfirmacoesNecessarias)
            Status = StatusReporte.Confirmado;
        MarcarAtualizacao();
    }

    public void RegistrarQueAguaBaixou()
    {
        GarantirQueNaoEstaEncerrado();
        ConfirmacoesDeBaixa++;
        if (ConfirmacoesDeBaixa >= ConfirmacoesNecessarias)
        {
            Status = StatusReporte.Encerrado;
            EncerradoEm = DateTime.UtcNow;
        }
        MarcarAtualizacao();
    }

    public override void Validar()
    {
        if (!Enum.IsDefined(Nivel))
            throw new DomainException("Nível de água inválido.");
        if (Comentario is { Length: > 280 })
            throw new DomainException("O comentário deve ter no máximo 280 caracteres.");
        _ = new Coordenada(Latitude, Longitude);
    }

    private void GarantirQueNaoEstaEncerrado()
    {
        if (Status == StatusReporte.Encerrado)
            throw new DomainException("Este reporte já foi encerrado.");
    }
}