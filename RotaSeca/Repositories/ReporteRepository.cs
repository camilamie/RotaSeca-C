using Microsoft.EntityFrameworkCore;
using RotaSeca.Data;
using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Enums;

namespace RotaSeca.Repositories
{
    public class ReporteRepository : Repository<Reporte>, IReporteRepository
    {
        public ReporteRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Reporte>> ListarAtivosAsync(DateTime desdeUtc, CancellationToken ct = default) =>
            await DbSet.AsNoTracking()
                       .Where(r => r.Status != StatusReporte.Encerrado && r.CriadoEm >= desdeUtc)
                       .OrderByDescending(r => r.CriadoEm)
                       .ToListAsync(ct);

        public async Task<IReadOnlyList<Reporte>> ListarPorUsuarioAsync(int usuarioId, int limite, CancellationToken ct = default) =>
            await DbSet.AsNoTracking()
                       .Where(r => r.UsuarioId == usuarioId)
                       .OrderByDescending(r => r.CriadoEm)
                       .Take(limite)
                       .ToListAsync(ct);
    }
}
