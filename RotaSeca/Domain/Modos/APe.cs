using RotaSeca.Domain.Enums;

namespace RotaSeca.Domain.Modos
{
    /// <summary>15 a 20 cm de água já derrubam uma pessoa: bloqueia a partir de 40%.</summary>
    public sealed class APe : ModoDeslocamentoBase
    {
        public override ModoDeslocamentoTipo Tipo => ModoDeslocamentoTipo.APe;
        public override string Nome => "A pé";
        public override double LimiteBloqueio => 0.40;
        protected override double VelocidadeMediaKmH => 5;
    }
}
