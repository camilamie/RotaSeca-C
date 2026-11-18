using RotaSeca.Domain.Enums;

namespace RotaSeca.Domain.Modos
{
    /// <summary>O motor apaga e a roda perde aderência: bloqueia a partir de 50%.</summary>
    public sealed class MotoBike : ModoDeslocamentoBase
    {
        public override ModoDeslocamentoTipo Tipo => ModoDeslocamentoTipo.MotoBike;
        public override string Nome => "Moto e bicicleta";
        public override double LimiteBloqueio => 0.50;
        protected override double VelocidadeMediaKmH => 18;
    }
}
