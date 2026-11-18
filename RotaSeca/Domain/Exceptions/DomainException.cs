namespace RotaSeca.Domain.Exceptions
{
    /// <summary>Regra de negócio violada. A API devolve HTTP 400.</summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
