using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<LugarSalvo> LugaresSalvos => Set<LugarSalvo>();
        public DbSet<Rota> Rotas => Set<Rota>();
        public DbSet<Reporte> Reportes => Set<Reporte>();
        public DbSet<Alerta> Alertas => Set<Alerta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var alteradas = ChangeTracker.Entries<EntidadeBase>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified);

            foreach (var entrada in alteradas)
                entrada.Entity.Validar();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
