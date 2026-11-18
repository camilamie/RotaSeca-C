using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<bool> ExisteAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<int>> ListarIdsComLugaresAsync(CancellationToken ct = default);
    }
}
