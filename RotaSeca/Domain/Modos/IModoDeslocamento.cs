using RotaSeca.Domain.Enums;

namespace RotaSeca.Domain.Modos
{
    public interface IModoDeslocamento
    {
        ModoDeslocamentoTipo Tipo { get; }
        string Nome { get; }
        double LimiteBloqueio { get; }
        bool DeveBloquear(double probabilidadeAlagamento);
        int EstimarDuracaoMinutos(double distanciaKm);
    }
}
