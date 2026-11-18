using Microsoft.EntityFrameworkCore;
using RotaSeca.Data;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public class AlertaRepository : Repository<Alerta>, IAlertaRepository
    {
        public AlertaRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Alerta>> ListarPorUsuarioAsync(int usuarioId, bool apenasNaoLidos, CancellationToken ct = default)
        {
            IQueryable<Alerta> consulta = DbSet.AsNoTracking()
                                               .Include(a => a.LugarSalvo)
                                               .Where(a => a.UsuarioId == usuarioId);

            if (apenasNaoLidos)
                consulta = consulta.Where(a => !a.Lido);

            return await consulta.OrderByDescending(a => a.CriadoEm).Take(50).ToListAsync(ct);
        }

        public Task<bool> ExisteAlertaRecenteAsync(int lugarSalvoId, DateTime desdeUtc, CancellationToken ct = default) =>
            DbSet.AnyAsync(a => a.LugarSalvoId == lugarSalvoId && a.CriadoEm >= desdeUtc, ct);
    }
}
