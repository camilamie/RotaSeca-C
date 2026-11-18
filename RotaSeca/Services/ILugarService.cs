using RotaSeca.DTOs;

namespace RotaSeca.Services
{
    public interface ILugarService
    {
        Task<LugarResponse> CriarAsync(SalvarLugarRequest request, CancellationToken ct = default);
        Task<IReadOnlyList<LugarResponse>> ListarAsync(int usuarioId, CancellationToken ct = default);
        Task<IReadOnlyList<LugarResponse>> ListarEmRiscoAsync(int usuarioId, double limite, CancellationToken ct = default);
        Task<LugarResponse> AtualizarAsync(int id, SalvarLugarRequest request, CancellationToken ct = default);
        Task RemoverAsync(int id, CancellationToken ct = default);
    }
}
