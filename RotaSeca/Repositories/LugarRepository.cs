using Microsoft.EntityFrameworkCore;
using RotaSeca.Data;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public class LugarRepository : Repository<LugarSalvo>, ILugarRepository
    {
        public LugarRepository(AppDbContext context) : base(context) { }

        
        public async Task<IReadOnlyList<LugarSalvo>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default) =>
            await DbSet.Where(l => l.UsuarioId == usuarioId)
                       .OrderBy(l => l.Etiqueta)
                       .ToListAsync(ct);

        public async Task<IReadOnlyList<LugarSalvo>> ListarEmRiscoAsync(int usuarioId, double limite, CancellationToken ct = default) =>
            await DbSet.AsNoTracking()
                       .Where(l => l.UsuarioId == usuarioId && l.UltimoRisco != null && l.UltimoRisco >= limite)
                       .OrderByDescending(l => l.UltimoRisco)
                       .ToListAsync(ct);

        public Task<int> ContarPorUsuarioAsync(int usuarioId, CancellationToken ct = default) =>
            DbSet.CountAsync(l => l.UsuarioId == usuarioId, ct);

        public Task<bool> EtiquetaEmUsoAsync(int usuarioId, string etiqueta, int? ignorarId = null, CancellationToken ct = default)
        {
            var normalizada = etiqueta.Trim().ToLower();
            return DbSet.AnyAsync(l => l.UsuarioId == usuarioId
                                       && l.Etiqueta.ToLower() == normalizada
                                       && (ignorarId == null || l.Id != ignorarId), ct);
        }
    }
}
