using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Enums;

namespace RotaSeca.Data.Configurations
{
    public class LugarSalvoConfiguration : IEntityTypeConfiguration<LugarSalvo>
    {
        public void Configure(EntityTypeBuilder<LugarSalvo> builder)
        {
            builder.ToTable("LugaresSalvos");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Etiqueta).IsRequired().HasMaxLength(40);
            builder.Property(l => l.Endereco).IsRequired().HasMaxLength(200);

            builder.HasIndex(l => new { l.UsuarioId, l.Etiqueta }).IsUnique();
            builder.HasIndex(l => l.UltimoRisco);

            builder.HasData(
                new
                {
                    Id = 1,
                    UsuarioId = 1,
                    Etiqueta = "Trabalho",
                    Endereco = "Endereço simulado - Vale do Anhangabaú, Centro",
                    Latitude = -23.5455,
                    Longitude = -46.6370,
                    Dias = DiasSemana.DiasUteis,
                    HorarioHabitual = (TimeOnly?)new TimeOnly(8, 30),
                    CriadoEm = SeedData.CriadoEm
                },
                new
                {
                    Id = 2,
                    UsuarioId = 1,
                    Etiqueta = "Casa",
                    Endereco = "Endereço simulado - Vila Mariana",
                    Latitude = -23.5890,
                    Longitude = -46.6340,
                    Dias = DiasSemana.TodosOsDias,
                    HorarioHabitual = (TimeOnly?)null,
                    CriadoEm = SeedData.CriadoEm
                });
        }
    }
}
