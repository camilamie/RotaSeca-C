using RotaSeca.DTOs;

namespace RotaSeca.Services
{
    public interface IReporteService
    {
        Task<ReporteResponse> CriarAsync(CriarReporteRequest request, CancellationToken ct = default);
        Task<ReporteResponse> ObterAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<ReporteResponse>> ListarAtivosAsync(int ultimasHoras, CancellationToken ct = default);
        Task<ReporteResponse> ConfirmarAsync(int id, CancellationToken ct = default);
        Task<ReporteResponse> RegistrarAguaBaixouAsync(int id, CancellationToken ct = default);
    }
}
