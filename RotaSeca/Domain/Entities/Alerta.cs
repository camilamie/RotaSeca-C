using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;

namespace RotaSeca.Domain.Entities
{
    public class Alerta : EntidadeBase
    {
        public int UsuarioId { get; private set; }
        public int LugarSalvoId { get; private set; }
        public LugarSalvo? LugarSalvo { get; private set; }
        public double Probabilidade { get; private set; }
        public NivelAlerta Nivel { get; private set; }
        public string Mensagem { get; private set; } = string.Empty;
        public bool Lido { get; private set; }

        protected Alerta() { }

        public Alerta(int usuarioId, LugarSalvo lugar, double probabilidade)
        {
            UsuarioId = usuarioId;
            LugarSalvoId = lugar.Id;
            Probabilidade = probabilidade;
            Nivel = ClassificarNivel(probabilidade);
            Mensagem = $"A região de {lugar.Etiqueta} tem {probabilidade * 100:0}% de chance de alagar na próxima hora.";
            Validar();
        }

        public static NivelAlerta ClassificarNivel(double probabilidade) => probabilidade switch
        {
            >= 0.8 => NivelAlerta.Critico,
            >= 0.65 => NivelAlerta.Alto,
            _ => NivelAlerta.Atencao
        };

        public void MarcarComoLido()
        {
            Lido = true;
            MarcarAtualizacao();
        }

        public override void Validar()
        {
            if (UsuarioId <= 0)
                throw new DomainException("Usuário inválido.");
            if (Probabilidade is < 0 or > 1)
                throw new DomainException("A probabilidade deve estar entre 0 e 1.");
        }
    }
}
