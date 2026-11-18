using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaSeca.Domain.Entities;

namespace RotaSeca.Data.Configurations
{
    public class AlertaConfiguration : IEntityTypeConfiguration<Alerta>
    {
        public void Configure(EntityTypeBuilder<Alerta> builder)
        {
            builder.ToTable("Alertas");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nivel).HasConversion<string>().HasMaxLength(20);
            builder.Property(a => a.Mensagem).IsRequired().HasMaxLength(300);

            builder.HasOne(a => a.LugarSalvo)
                   .WithMany()
                   .HasForeignKey(a => a.LugarSalvoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey(a => a.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict); // evita dois caminhos de exclusão em cascata

            builder.HasIndex(a => new { a.UsuarioId, a.Lido });
        }
    }
}
