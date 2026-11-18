using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.ValueObjects;
using RotaSeca.DTOs;
using RotaSeca.Repositories;

namespace RotaSeca.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reportes;
        private readonly IUsuarioRepository _usuarios;

        public ReporteService(IReporteRepository reportes, IUsuarioRepository usuarios)
        {
            _reportes = reportes;
            _usuarios = usuarios;
        }

        public async Task<ReporteResponse> CriarAsync(CriarReporteRequest request, CancellationToken ct = default)
        {
            if (request.UsuarioId is int usuarioId && !await _usuarios.ExisteAsync(usuarioId, ct))
                throw new NotFoundException($"Usuário {usuarioId} não encontrado.");

            var reporte = new Reporte(request.UsuarioId, new Coordenada(request.Latitude, request.Longitude),
                                      request.Nivel, request.Comentario);

            await _reportes.AdicionarAsync(reporte, ct);
            await _reportes.SalvarAsync(ct);
            return reporte.ToResponse();
        }

        public async Task<ReporteResponse> ObterAsync(int id, CancellationToken ct = default) =>
            (await BuscarAsync(id, ct)).ToResponse();

        public async Task<IReadOnlyList<ReporteResponse>> ListarAtivosAsync(int ultimasHoras, CancellationToken ct = default)
        {
            if (ultimasHoras is < 1 or > 72)
                throw new DomainException("Informe um período entre 1 e 72 horas.");

            var reportes = await _reportes.ListarAtivosAsync(DateTime.UtcNow.AddHours(-ultimasHoras), ct);
            return reportes.Select(r => r.ToResponse()).ToList();
        }

        public async Task<ReporteResponse> ConfirmarAsync(int id, CancellationToken ct = default)
        {
            var reporte = await BuscarAsync(id, ct);
            reporte.Confirmar();
            await _reportes.SalvarAsync(ct);
            return reporte.ToResponse();
        }

        public async Task<ReporteResponse> RegistrarAguaBaixouAsync(int id, CancellationToken ct = default)
        {
            var reporte = await BuscarAsync(id, ct);
            reporte.RegistrarQueAguaBaixou();
            await _reportes.SalvarAsync(ct);
            return reporte.ToResponse();
        }

        private async Task<Reporte> BuscarAsync(int id, CancellationToken ct) =>
            await _reportes.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException($"Reporte {id} não encontrado.");
    }
}
