namespace RotaSeca.Domain.Exceptions
{
    /// <summary>Recurso não encontrado. A API devolve HTTP 404.</summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
