using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations
{
    public interface IChuvaProvider
    {
        Task<ChuvaAtual> ObterChuvaAsync(Coordenada ponto, CancellationToken ct = default);
    }
}
