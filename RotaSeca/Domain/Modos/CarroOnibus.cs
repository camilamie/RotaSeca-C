using RotaSeca.Domain.Enums;

namespace RotaSeca.Domain.Modos
{
    /// <summary>Tolera mais lâmina d'água: bloqueia a partir de 70%.</summary>
    public sealed class CarroOnibus : ModoDeslocamentoBase
    {
        private const int MinutosParadasETransito = 5;

        public override ModoDeslocamentoTipo Tipo => ModoDeslocamentoTipo.CarroOnibus;
        public override string Nome => "Carro e ônibus";
        public override double LimiteBloqueio => 0.70;
        protected override double VelocidadeMediaKmH => 22;

        
        public override int EstimarDuracaoMinutos(double distanciaKm) =>
            base.EstimarDuracaoMinutos(distanciaKm) + MinutosParadasETransito;
    }
}
