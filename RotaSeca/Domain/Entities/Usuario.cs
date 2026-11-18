
using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;

namespace RotaSeca.Domain.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; private set; } = string.Empty;
        public ModoDeslocamentoTipo ModoPreferido { get; private set; }

        private readonly List<LugarSalvo> _lugares = new();
        public IReadOnlyCollection<LugarSalvo> Lugares => _lugares;

        protected Usuario() { } 
       
        public Usuario(string nome, ModoDeslocamentoTipo modoPreferido)
        {
            Nome = nome?.Trim() ?? string.Empty;
            ModoPreferido = modoPreferido;
            Validar();
        }

        public void AlterarModoPreferido(ModoDeslocamentoTipo modo)
        {
            ModoPreferido = modo;
            Validar();
            MarcarAtualizacao();
        }

        public override void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome) || Nome.Length < 2)
                throw new DomainException("O nome deve ter pelo menos 2 caracteres.");
            if (Nome.Length > 80)
                throw new DomainException("O nome deve ter no máximo 80 caracteres.");
            if (!Enum.IsDefined(ModoPreferido))
                throw new DomainException("Modo de deslocamento inválido.");
        }
    }
}
