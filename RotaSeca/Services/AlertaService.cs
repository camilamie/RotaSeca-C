using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Exceptions;
using RotaSeca.DTOs;
using RotaSeca.Integrations;
using RotaSeca.Repositories;

namespace RotaSeca.Services
{
    public class AlertaService : IAlertaService
    {
        public const double LimiteAlerta = 0.5;

        private readonly IAlertaRepository _alertas;
        private readonly ILugarRepository _lugares;
        private readonly IRiscoProvider _risco;

        public AlertaService(IAlertaRepository alertas, ILugarRepository lugares, IRiscoProvider risco)
        {
            _alertas = alertas;
            _lugares = lugares;
            _risco = risco;
        }

        public async Task<int> GerarAlertasAsync(int usuarioId, CancellationToken ct = default)
        {
            var lugares = await _lugares.ListarPorUsuarioAsync(usuarioId, ct);
            var proximaHora = DateTime.UtcNow.AddHours(1);
            var hojeEmSaoPaulo = DateTime.UtcNow.AddHours(-3).DayOfWeek;
            var gerados = 0;

            foreach (var lugar in lugares.Where(l => l.FrequentaNoDia(hojeEmSaoPaulo)))
            {
                var probabilidade = await _risco.ObterProbabilidadeAsync(lugar.ObterCoordenada(), proximaHora, ct);
                lugar.RegistrarRisco(probabilidade);

                if (probabilidade < LimiteAlerta)
                    continue;

               
                if (await _alertas.ExisteAlertaRecenteAsync(lugar.Id, DateTime.UtcNow.AddHours(-1), ct))
                    continue;

                await _alertas.AdicionarAsync(new Alerta(usuarioId, lugar, probabilidade), ct);
                gerados++;
            }

            await _alertas.SalvarAsync(ct); 
            return gerados;
        }

        public async Task<IReadOnlyList<AlertaResponse>> ListarAsync(int usuarioId, bool apenasNaoLidos, CancellationToken ct = default)
        {
            var alertas = await _alertas.ListarPorUsuarioAsync(usuarioId, apenasNaoLidos, ct);
            return alertas.Select(a => a.ToResponse()).ToList();
        }

        public async Task MarcarComoLidoAsync(int id, CancellationToken ct = default)
        {
            var alerta = await _alertas.ObterPorIdAsync(id, ct)
                ?? throw new NotFoundException($"Alerta {id} não encontrado.");
            alerta.MarcarComoLido();
            await _alertas.SalvarAsync(ct);
        }
    }
}
