using Microsoft.Extensions.Options;
using RotaSeca.Integrations;
using RotaSeca.Repositories;
using RotaSeca.Services;

namespace RotaSeca.BackgroundJobs
{
    public class AlertaBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AlertaBackgroundService> _logger;
        private readonly TimeSpan _intervalo;

        public AlertaBackgroundService(IServiceScopeFactory scopeFactory, IOptions<ServicoRiscoOptions> opcoes,
                                       ILogger<AlertaBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _intervalo = TimeSpan.FromMinutes(Math.Max(1, opcoes.Value.IntervaloAlertasMinutos));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_intervalo);

            do
            {
                try
                {
                    
                    using var scope = _scopeFactory.CreateScope();
                    var usuarios = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
                    var alertas = scope.ServiceProvider.GetRequiredService<IAlertaService>();

                    foreach (var usuarioId in await usuarios.ListarIdsComLugaresAsync(stoppingToken))
                    {
                        var gerados = await alertas.GerarAlertasAsync(usuarioId, stoppingToken);
                        if (gerados > 0)
                            _logger.LogInformation("{Quantidade} alerta(s) gerado(s) para o usuário {UsuarioId}.",
                                                   gerados, usuarioId);
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Falha ao gerar alertas automáticos.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
