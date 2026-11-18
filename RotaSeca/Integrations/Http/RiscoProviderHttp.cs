using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations.Http
{
    /// <summary>
    /// Consulta o Serviço de Risco da Rua (entrega de SOA).
    /// Ajuste o caminho e os campos ao contrato combinado com o grupo.
    /// </summary>
    public class RiscoProviderHttp : IRiscoProvider
    {
        private readonly HttpClient _http;

        public RiscoProviderHttp(HttpClient http) => _http = http;

        public async Task<double> ObterProbabilidadeAsync(Coordenada ponto, DateTime momentoUtc,
                                                          CancellationToken ct = default)
        {
            
            var url = FormattableString.Invariant(
                $"risco/ponto?lat={ponto.Latitude}&lon={ponto.Longitude}&momento={momentoUtc:O}");

            var resposta = await _http.GetFromJsonAsync<RiscoPontoResponse>(url, ct)
                ?? throw new InvalidOperationException("Resposta vazia do Serviço de Risco da Rua.");

            return resposta.Probabilidade;
        }

        private sealed record RiscoPontoResponse(double Probabilidade);
    }
}
