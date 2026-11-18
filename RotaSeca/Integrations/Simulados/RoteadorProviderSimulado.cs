using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations.Simulados
{
    public class RoteadorProviderSimulado : IRoteadorProvider
    {
        private const int Segmentos = 6;
        private const double FatorMalhaViaria = 1.25; // ruas não são linha reta

        public async Task<IReadOnlyList<RotaCandidata>> CalcularRotasAsync(
            Coordenada origem, Coordenada destino, CancellationToken ct = default)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(50), ct);

            var variacoes = new[]
            {
            (Nome: "Via direta", Desvio: 0.0),
            (Nome: "Alternativa 1", Desvio: 0.012),  // ~1,3 km para um lado
            (Nome: "Alternativa 2", Desvio: -0.012)  // ~1,3 km para o outro
        };

            return variacoes.Select(v => MontarRota(v.Nome, origem, destino, v.Desvio)).ToList();
        }

        private static RotaCandidata MontarRota(string nome, Coordenada origem, Coordenada destino, double desvio)
        {
            var dLat = destino.Latitude - origem.Latitude;
            var dLon = destino.Longitude - origem.Longitude;
            var comprimento = Math.Sqrt(dLat * dLat + dLon * dLon);

            // Vetor perpendicular ao trajeto, para a curva ir "para o lado"
            var (perpLat, perpLon) = comprimento == 0 ? (0.0, 0.0) : (-dLon / comprimento, dLat / comprimento);

            var pontos = Enumerable.Range(0, Segmentos + 1)
                .Select(i =>
                {
                    var t = (double)i / Segmentos;
                    var curva = Math.Sin(Math.PI * t) * desvio; // zero nas pontas, máximo no meio
                    return new Coordenada(origem.Latitude + dLat * t + perpLat * curva,
                                          origem.Longitude + dLon * t + perpLon * curva);
                })
                .ToList();

            var distancia = pontos.Zip(pontos.Skip(1), (a, b) => a.DistanciaKmAte(b)).Sum() * FatorMalhaViaria;
            return new RotaCandidata(nome, Math.Round(distancia, 2), pontos);
        }
    }
}
