using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations
{
    /// <summary>Probabilidade (0 a 1) de um ponto alagar num determinado momento.</summary>
    public interface IRiscoProvider
    {
        Task<double> ObterProbabilidadeAsync(Coordenada ponto, DateTime momentoUtc,
                                             CancellationToken ct = default);
    }
}
