using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public interface IAlertaRepository : IRepository<Alerta>
    {
        Task<IReadOnlyList<Alerta>> ListarPorUsuarioAsync(int usuarioId, bool apenasNaoLidos, CancellationToken ct = default);
        Task<bool> ExisteAlertaRecenteAsync(int lugarSalvoId, DateTime desdeUtc, CancellationToken ct = default);
    }
}
