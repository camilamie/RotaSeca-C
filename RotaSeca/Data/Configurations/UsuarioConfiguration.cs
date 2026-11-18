using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Enums;

namespace RotaSeca.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Nome).IsRequired().HasMaxLength(80);
            builder.Property(u => u.ModoPreferido).HasConversion<string>().HasMaxLength(20);

            builder.HasMany(u => u.Lugares)
                   .WithOne(l => l.Usuario)
                   .HasForeignKey(l => l.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(u => u.Lugares).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasData(
                new { Id = 1, Nome = "Usuária Teste A (simulada)", ModoPreferido = ModoDeslocamentoTipo.APe, CriadoEm = SeedData.CriadoEm },
                new { Id = 2, Nome = "Usuário Teste B (simulado)", ModoPreferido = ModoDeslocamentoTipo.MotoBike, CriadoEm = SeedData.CriadoEm });
        }
    }
}
