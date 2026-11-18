using RotaSeca.Domain.Enums;

namespace RotaSeca.DTOs
{
    public record SalvarLugarRequest(
    int UsuarioId,
    string Etiqueta,
    string Endereco,
    double Latitude,
    double Longitude,
    DiasSemana Dias = DiasSemana.Nenhum,
    TimeOnly? HorarioHabitual = null);

    public record LugarResponse(
        int Id,
        string Etiqueta,
        string Endereco,
        double Latitude,
        double Longitude,
        DiasSemana Dias,
        TimeOnly? HorarioHabitual,
        double? Risco,
        string CorRisco,
        DateTime? RiscoAtualizadoEm);
}
