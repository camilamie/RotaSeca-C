using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations
{
    /// <summary>Gera rotas candidatas.</summary>
    public interface IRoteadorProvider
    {
        Task<IReadOnlyList<RotaCandidata>> CalcularRotasAsync(Coordenada origem, Coordenada destino,
                                                              CancellationToken ct = default);
    }
}
