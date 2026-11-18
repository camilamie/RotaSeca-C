using RotaSeca.Domain.Entities;
using RotaSeca.DTOs;

namespace RotaSeca.Repositories
{
    public interface IRotaRepository : IRepository<Rota>
    {
        Task<IReadOnlyList<Rota>> ListarPorUsuarioAsync(int usuarioId, int limite, CancellationToken ct = default);
        Task<ResumoHistorico> ObterResumoAsync(int usuarioId, CancellationToken ct = default);
    }
}
