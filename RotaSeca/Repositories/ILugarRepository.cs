using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public interface ILugarRepository : IRepository<LugarSalvo>
    {
        Task<IReadOnlyList<LugarSalvo>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
        Task<IReadOnlyList<LugarSalvo>> ListarEmRiscoAsync(int usuarioId, double limite, CancellationToken ct = default);
        Task<int> ContarPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
        Task<bool> EtiquetaEmUsoAsync(int usuarioId, string etiqueta, int? ignorarId = null, CancellationToken ct = default);
    }
}
