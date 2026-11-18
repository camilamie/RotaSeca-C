namespace RotaSeca.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public int Id { get; protected set; }
        public DateTime CriadoEm { get; protected set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; protected set; }

        protected void MarcarAtualizacao() => AtualizadoEm = DateTime.UtcNow;

        /// <summary>Cada entidade define as próprias regras.</summary>
        public abstract void Validar();
    }
}
