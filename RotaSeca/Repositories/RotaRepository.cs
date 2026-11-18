using Microsoft.EntityFrameworkCore;
using RotaSeca.Data;
using RotaSeca.Domain.Entities;
using RotaSeca.DTOs;

namespace RotaSeca.Repositories
{
    public class RotaRepository : Repository<Rota>, IRotaRepository
    {
        public RotaRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Rota>> ListarPorUsuarioAsync(int usuarioId, int limite, CancellationToken ct = default) =>
            await DbSet.AsNoTracking()
                       .Where(r => r.UsuarioId == usuarioId)
                       .OrderByDescending(r => r.CriadoEm)
                       .Take(limite)
                       .ToListAsync(ct);

        public async Task<ResumoHistorico> ObterResumoAsync(int usuarioId, CancellationToken ct = default)
        {
            var consultas = DbSet.AsNoTracking().Where(r => r.UsuarioId == usuarioId);

            var total = await consultas.CountAsync(ct);
            var comDesvio = await consultas.CountAsync(r => r.OpcoesBloqueadas > 0 && !r.SemCaminhoSeco, ct);
            var semCaminho = await consultas.CountAsync(r => r.SemCaminhoSeco, ct);

            var porModo = await consultas
                .GroupBy(r => r.Modo)
                .Select(g => new { Modo = g.Key, Quantidade = g.Count() })
                .OrderByDescending(x => x.Quantidade)
                .ToListAsync(ct);

            var duracaoMedia = await consultas
                .Where(r => r.DuracaoMinutos != null)
                .AverageAsync(r => (double?)r.DuracaoMinutos, ct);

            var percentual = total == 0 ? 0 : Math.Round(comDesvio * 100.0 / total, 1);

            return new ResumoHistorico(
                total,
                comDesvio,
                percentual,
                semCaminho,
                duracaoMedia is null ? null : Math.Round(duracaoMedia.Value, 1),
                porModo.Select(x => new ContagemPorModo(x.Modo, x.Quantidade)).ToList());
        }
    }
}
