using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Data.Configurations
{
    public class ReporteConfiguration : IEntityTypeConfiguration<Reporte>
    {
        public void Configure(EntityTypeBuilder<Reporte> builder)
        {
            builder.ToTable("Reportes");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Nivel).HasConversion<string>().HasMaxLength(20);
            builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(r => r.Comentario).HasMaxLength(280);

            builder.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey(r => r.UsuarioId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(r => new { r.Status, r.CriadoEm });
        }
    }
}
