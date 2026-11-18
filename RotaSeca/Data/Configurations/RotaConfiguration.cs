using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Data.Configurations
{
    public class RotaConfiguration : IEntityTypeConfiguration<Rota>
    {
        public void Configure(EntityTypeBuilder<Rota> builder)
        {
            builder.ToTable("Rotas");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Modo).HasConversion<string>().HasMaxLength(20);
            builder.Property(r => r.DescricaoRecomendada).HasMaxLength(60);
            builder.Ignore(r => r.DesviouDeRisco); // propriedade calculada: não vira coluna

            builder.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey(r => r.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => new { r.UsuarioId, r.CriadoEm });
        }
    }
}
