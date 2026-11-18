using RotaSeca.Domain.Exceptions;
using RotaSeca.DTOs;
using RotaSeca.Repositories;

namespace RotaSeca.Services
{
    public class HistoricoService : IHistoricoService
    {
        private const int Limite = 10;

        private readonly IRotaRepository _rotas;
        private readonly IReporteRepository _reportes;
        private readonly IUsuarioRepository _usuarios;

        public HistoricoService(IRotaRepository rotas, IReporteRepository reportes, IUsuarioRepository usuarios)
        {
            _rotas = rotas;
            _reportes = reportes;
            _usuarios = usuarios;
        }

        public async Task<HistoricoResponse> ObterAsync(int usuarioId, CancellationToken ct = default)
        {
            if (!await _usuarios.ExisteAsync(usuarioId, ct))
                throw new NotFoundException($"Usuário {usuarioId} não encontrado.");

            
            var resumo = await _rotas.ObterResumoAsync(usuarioId, ct);
            var rotas = await _rotas.ListarPorUsuarioAsync(usuarioId, Limite, ct);
            var reportes = await _reportes.ListarPorUsuarioAsync(usuarioId, Limite, ct);

            return new HistoricoResponse(resumo,
                                         rotas.Select(r => r.ToHistorico()).ToList(),
                                         reportes.Select(r => r.ToResponse()).ToList());
        }
    }
}
