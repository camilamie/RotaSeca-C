using RotaSeca.DTOs;

namespace RotaSeca.Services
{
    public interface IAlertaService
    {
        Task<int> GerarAlertasAsync(int usuarioId, CancellationToken ct = default);
        Task<IReadOnlyList<AlertaResponse>> ListarAsync(int usuarioId, bool apenasNaoLidos, CancellationToken ct = default);
        Task MarcarComoLidoAsync(int id, CancellationToken ct = default);
    }
}
