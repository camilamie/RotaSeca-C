using Microsoft.Extensions.Options;
using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations.Simulados
{
    public class ChuvaProviderSimulado : IChuvaProvider
    {
        private readonly ServicoRiscoOptions _opcoes;

        public ChuvaProviderSimulado(IOptions<ServicoRiscoOptions> opcoes) => _opcoes = opcoes.Value;

        public async Task<ChuvaAtual> ObterChuvaAsync(Coordenada ponto, CancellationToken ct = default)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(20), ct);
            var intensidade = _opcoes.ChuvaSimuladaMmH;
            return new ChuvaAtual(intensidade, Math.Round(intensidade * 1.2, 1), DateTime.UtcNow, "Simulado");
        }
    }
}
