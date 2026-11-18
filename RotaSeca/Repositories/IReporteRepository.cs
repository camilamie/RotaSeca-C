using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public interface IReporteRepository : IRepository<Reporte>
    {
        Task<IReadOnlyList<Reporte>> ListarAtivosAsync(DateTime desdeUtc, CancellationToken ct = default);
        Task<IReadOnlyList<Reporte>> ListarPorUsuarioAsync(int usuarioId, int limite, CancellationToken ct = default);
    }
}
