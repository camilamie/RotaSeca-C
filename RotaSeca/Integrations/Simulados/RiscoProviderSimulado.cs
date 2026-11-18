using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations.Simulados
{
    public class RiscoProviderSimulado : IRiscoProvider
    {
       
        private static readonly Coordenada[] PontosCriticos =
        {
        new(-23.5450, -46.6380), // Vale do Anhangabaú
        new(-23.5490, -46.6420), // Praça da Bandeira
        new(-23.5270, -46.6340), // Região da Av. do Estado
        new(-23.5180, -46.6250)  // Marginal Tietê
    };

        private const double RaioInfluenciaKm = 1.2;

        public async Task<double> ObterProbabilidadeAsync(Coordenada ponto, DateTime momentoUtc,
                                                          CancellationToken ct = default)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(30), ct); // simula a latência da rede

            var distanciaMaisProxima = PontosCriticos.Min(p => p.DistanciaKmAte(ponto)); // LINQ
            var proximidade = Math.Max(0, 1 - distanciaMaisProxima / RaioInfluenciaKm);

            // Chuvas de verão em SP costumam ser no fim da tarde
            var horaLocal = momentoUtc.AddHours(-3).Hour;
            var fatorHorario = horaLocal is >= 14 and <= 21 ? 1.0 : 0.75;

            return Math.Round(Math.Clamp(0.05 + proximidade * 0.95 * fatorHorario, 0, 1), 2);
        }
    }
}
