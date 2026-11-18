using RotaSeca.Domain.Enums;

namespace RotaSeca.DTOs
{
    public record CriarReporteRequest(int? UsuarioId, double Latitude, double Longitude,
                                  NivelAgua Nivel, string? Comentario = null);

    public record ReporteResponse(int Id, double Latitude, double Longitude, NivelAgua Nivel,
                                  StatusReporte Status, int Confirmacoes, string? Comentario,
                                  DateTime CriadoEm);
}
