
using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Domain.Entities
{
    public class LugarSalvo : EntidadeBase
    {
        public const int MaximoPorUsuario = 10;

        public int UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; }
        public string Etiqueta { get; private set; } = string.Empty;
        public string Endereco { get; private set; } = string.Empty;
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public DiasSemana Dias { get; private set; }
        public TimeOnly? HorarioHabitual { get; private set; }
        public double? UltimoRisco { get; private set; }
        public DateTime? RiscoAtualizadoEm { get; private set; }

        protected LugarSalvo() { }

        public LugarSalvo(int usuarioId, string etiqueta, string endereco, Coordenada coordenada,
                          DiasSemana dias = DiasSemana.Nenhum, TimeOnly? horarioHabitual = null)
        {
            UsuarioId = usuarioId;
            DefinirDados(etiqueta, endereco, coordenada, dias, horarioHabitual);
            Validar();
        }

        public void Atualizar(string etiqueta, string endereco, Coordenada coordenada,
                              DiasSemana dias, TimeOnly? horarioHabitual)
        {
            DefinirDados(etiqueta, endereco, coordenada, dias, horarioHabitual);
            Validar();
            MarcarAtualizacao();
        }

        public void RegistrarRisco(double probabilidade)
        {
            if (probabilidade is < 0 or > 1)
                throw new DomainException("A probabilidade deve estar entre 0 e 1.");
            UltimoRisco = probabilidade;
            RiscoAtualizadoEm = DateTime.UtcNow;
        }

        /// <summary>Sem dias cadastrados, o app vigia o lugar todos os dias.</summary>
        public bool FrequentaNoDia(DayOfWeek dia)
        {
            var flag = (DiasSemana)(1 << (int)dia); // Domingo = 1, Segunda = 2, ...
            return Dias == DiasSemana.Nenhum || Dias.HasFlag(flag);
        }

        public Coordenada ObterCoordenada() => new(Latitude, Longitude);

        public override void Validar()
        {
            if (UsuarioId <= 0)
                throw new DomainException("O lugar precisa pertencer a um usuário.");
            if (string.IsNullOrWhiteSpace(Etiqueta) || Etiqueta.Length > 40)
                throw new DomainException("A etiqueta é obrigatória e deve ter até 40 caracteres.");
            if (string.IsNullOrWhiteSpace(Endereco) || Endereco.Length < 5 || Endereco.Length > 200)
                throw new DomainException("O endereço deve ter entre 5 e 200 caracteres.");
            if ((Dias & ~DiasSemana.TodosOsDias) != 0)
                throw new DomainException("Dias da semana inválidos.");
            _ = ObterCoordenada(); 
        }

        private void DefinirDados(string etiqueta, string endereco, Coordenada coordenada,
                                  DiasSemana dias, TimeOnly? horario)
        {
            Etiqueta = etiqueta?.Trim() ?? string.Empty;
            Endereco = endereco?.Trim() ?? string.Empty;
            Latitude = coordenada.Latitude;
            Longitude = coordenada.Longitude;
            Dias = dias;
            HorarioHabitual = horario;
        }
    }
}
