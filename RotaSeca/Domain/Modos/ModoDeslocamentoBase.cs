using RotaSeca.Domain.Enums;

namespace RotaSeca.Domain.Modos
{
    public abstract class ModoDeslocamentoBase : IModoDeslocamento
    {
        public abstract ModoDeslocamentoTipo Tipo { get; }
        public abstract string Nome { get; }
        public abstract double LimiteBloqueio { get; }
        protected abstract double VelocidadeMediaKmH { get; }

        public virtual bool DeveBloquear(double probabilidadeAlagamento) =>
            probabilidadeAlagamento >= LimiteBloqueio;

        public virtual int EstimarDuracaoMinutos(double distanciaKm) =>
            (int)Math.Ceiling(distanciaKm / VelocidadeMediaKmH * 60);
    }
}
