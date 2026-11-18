using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.Modos;
using RotaSeca.Domain.ValueObjects;
using RotaSeca.DTOs;
using RotaSeca.Integrations;
using RotaSeca.Repositories;
using RotaSeca.Services.Models;

namespace RotaSeca.Services
{
    public class RotaService : IRotaService
    {
        public const int MaximoOpcoes = 3;

        private readonly IRotaRepository _rotas;
        private readonly IUsuarioRepository _usuarios;
        private readonly IRoteadorProvider _roteador;
        private readonly IRiscoProvider _risco;
        private readonly IChuvaProvider _chuva;
        private readonly IModoDeslocamentoFactory _modos;
        private readonly ILogger<RotaService> _logger;

        public RotaService(IRotaRepository rotas, IUsuarioRepository usuarios, IRoteadorProvider roteador,
                           IRiscoProvider risco, IChuvaProvider chuva, IModoDeslocamentoFactory modos,
                           ILogger<RotaService> logger)
        {
            _rotas = rotas;
            _usuarios = usuarios;
            _roteador = roteador;
            _risco = risco;
            _chuva = chuva;
            _modos = modos;
            _logger = logger;
        }

        public async Task<RotaCalculadaResponse> CalcularAsync(CalcularRotaRequest request, CancellationToken ct = default)
        {
            if (!await _usuarios.ExisteAsync(request.UsuarioId, ct))
                throw new NotFoundException($"Usuário {request.UsuarioId} não encontrado.");

            if (request.Origem is null || request.Destino is null)
                throw new DomainException("Informe a origem e o destino.");

            var modo = _modos.Obter(request.Modo);
            var origem = new Coordenada(request.Origem.Latitude, request.Origem.Longitude);
            var destino = new Coordenada(request.Destino.Latitude, request.Destino.Longitude);
            var consulta = new Rota(request.UsuarioId, origem, destino, modo.Tipo); // valida origem != destino

            // Roteador e chuva são independentes: rodam ao mesmo tempo
            var tarefaCandidatas = _roteador.CalcularRotasAsync(origem, destino, ct);
            var tarefaChuva = _chuva.ObterChuvaAsync(origem, ct);
            await Task.WhenAll(tarefaCandidatas, tarefaChuva);

            var candidatas = (await tarefaCandidatas).Take(MaximoOpcoes).ToList();
            var chuva = await tarefaChuva;

            if (candidatas.Count == 0)
                throw new DomainException("O roteador não encontrou nenhum caminho entre origem e destino.");

            var avaliadas = await Task.WhenAll(candidatas.Select(c => AvaliarAsync(c, modo, ct)));

            // LINQ: entre as liberadas, a de menor risco e, no empate, a mais rápida
            var recomendada = avaliadas
                .Where(o => !o.Bloqueada)
                .OrderBy(o => o.RiscoMaximo)
                .ThenBy(o => o.DuracaoMinutos)
                .FirstOrDefault();

            var bloqueadas = avaliadas.Count(o => o.Bloqueada);

            if (recomendada is null)
                consulta.RegistrarSemCaminhoSeco(avaliadas.Length);
            else
                consulta.RegistrarRecomendacao(avaliadas.Length, bloqueadas, recomendada.Descricao,
                                               recomendada.DistanciaKm, recomendada.DuracaoMinutos, recomendada.RiscoMaximo);

            await _rotas.AdicionarAsync(consulta, ct);
            await _rotas.SalvarAsync(ct);

            _logger.LogInformation("Rota calculada ({Modo}): {Bloqueadas}/{Total} bloqueadas.",
                                   modo.Nome, bloqueadas, avaliadas.Length);

            var opcoes = avaliadas
                .Select(o => o.ToResponse(recomendada: ReferenceEquals(o, recomendada)))
                .OrderByDescending(o => o.Recomendada)
                .ThenBy(o => o.Bloqueada)
                .ThenBy(o => o.DuracaoMinutos)
                .ToList();

            return new RotaCalculadaResponse(consulta.Id, modo.Nome, modo.LimiteBloqueio, chuva.IntensidadeMmH,
                                             SemCaminhoSeco: recomendada is null,
                                             MontarMensagem(modo, recomendada, bloqueadas), opcoes);
        }

        private async Task<OpcaoAvaliada> AvaliarAsync(RotaCandidata candidata, IModoDeslocamento modo, CancellationToken ct)
        {
            var agora = DateTime.UtcNow;
            var riscos = await Task.WhenAll(
                candidata.Pontos.Select(p => _risco.ObterProbabilidadeAsync(p, agora, ct)));

            var riscoMaximo = riscos.DefaultIfEmpty(0).Max();

            return new OpcaoAvaliada(candidata.Descricao, candidata.DistanciaKm,
                                     modo.EstimarDuracaoMinutos(candidata.DistanciaKm),
                                     riscoMaximo, modo.DeveBloquear(riscoMaximo), candidata.Pontos);
        }

        private static string MontarMensagem(IModoDeslocamento modo, OpcaoAvaliada? recomendada, int bloqueadas) =>
            recomendada is null
                ? $"Nenhum caminho seco disponível agora para o modo {modo.Nome}. Se puder, espere a chuva diminuir."
                : bloqueadas > 0
                    ? $"{bloqueadas} rota(s) marcada(s) como \"não dá\". Siga pela rota recomendada."
                    : "Todas as rotas estão dentro do limite de segurança.";
    }
}
