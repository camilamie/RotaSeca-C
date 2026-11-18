using RotaSeca.DTOs;

namespace RotaSeca.Services
{
    public interface IHistoricoService
    {
        Task<HistoricoResponse> ObterAsync(int usuarioId, CancellationToken ct = default);
    }
}
