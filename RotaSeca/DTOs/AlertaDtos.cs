using RotaSeca.Domain.Enums;

namespace RotaSeca.DTOs
{
    public record AlertaResponse(int Id, int LugarSalvoId, string? Etiqueta, double Probabilidade,
                              NivelAlerta Nivel, string Mensagem, bool Lido, DateTime CriadoEm);

    public record AlertasGeradosResponse(int AlertasGerados);
}
