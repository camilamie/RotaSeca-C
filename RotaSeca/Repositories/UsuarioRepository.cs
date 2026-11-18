using Microsoft.EntityFrameworkCore;
using RotaSeca.Data;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public Task<bool> ExisteAsync(int id, CancellationToken ct = default) =>
            DbSet.AnyAsync(u => u.Id == id, ct);

        public async Task<IReadOnlyList<int>> ListarIdsComLugaresAsync(CancellationToken ct = default) =>
            await DbSet.AsNoTracking()
                       .Where(u => u.Lugares.Any())
                       .Select(u => u.Id)
                       .ToListAsync(ct);
    }
}
