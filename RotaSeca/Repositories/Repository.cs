using Microsoft.EntityFrameworkCore;
using RotaSeca.Data;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntidadeBase
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default) =>
            DbSet.FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task AdicionarAsync(T entidade, CancellationToken ct = default) =>
            await DbSet.AddAsync(entidade, ct);

        public void Remover(T entidade) => DbSet.Remove(entidade);

        public Task<int> SalvarAsync(CancellationToken ct = default) => Context.SaveChangesAsync(ct);
    }
}
