using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;

namespace RotaSeca.Domain.Modos
{
    public interface IModoDeslocamentoFactory
    {
        IModoDeslocamento Obter(ModoDeslocamentoTipo tipo);
    }

    public class ModoDeslocamentoFactory : IModoDeslocamentoFactory
    {
        private readonly IReadOnlyDictionary<ModoDeslocamentoTipo, IModoDeslocamento> _modos;

        public ModoDeslocamentoFactory(IEnumerable<IModoDeslocamento> modos) =>
            _modos = modos.ToDictionary(m => m.Tipo);

        public IModoDeslocamento Obter(ModoDeslocamentoTipo tipo) =>
            _modos.TryGetValue(tipo, out var modo)
                ? modo
                : throw new DomainException($"Modo de deslocamento '{tipo}' não suportado.");
    }
}
