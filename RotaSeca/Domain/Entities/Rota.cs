
using RotaSeca.Domain.Enums;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Domain.Entities
{
    public class Rota : EntidadeBase
    {
        public int UsuarioId { get; private set; }
        public double OrigemLatitude { get; private set; }
        public double OrigemLongitude { get; private set; }
        public double DestinoLatitude { get; private set; }
        public double DestinoLongitude { get; private set; }
        public ModoDeslocamentoTipo Modo { get; private set; }

        public int OpcoesAvaliadas { get; private set; }
        public int OpcoesBloqueadas { get; private set; }
        public bool SemCaminhoSeco { get; private set; }
        public string? DescricaoRecomendada { get; private set; }
        public double? DistanciaKm { get; private set; }
        public int? DuracaoMinutos { get; private set; }
        public double? RiscoMaximo { get; private set; }

        /// <summary>Houve trecho bloqueado, mas foi possível desviar dele.</summary>
        public bool DesviouDeRisco => OpcoesBloqueadas > 0 && !SemCaminhoSeco;

        protected Rota() { }

        public Rota(int usuarioId, Coordenada origem, Coordenada destino, ModoDeslocamentoTipo modo)
        {
            UsuarioId = usuarioId;
            OrigemLatitude = origem.Latitude;
            OrigemLongitude = origem.Longitude;
            DestinoLatitude = destino.Latitude;
            DestinoLongitude = destino.Longitude;
            Modo = modo;
            Validar();
        }

        public void RegistrarRecomendacao(int avaliadas, int bloqueadas, string descricao,
                                          double distanciaKm, int duracaoMinutos, double riscoMaximo)
        {
            OpcoesAvaliadas = avaliadas;
            OpcoesBloqueadas = bloqueadas;
            SemCaminhoSeco = false;
            DescricaoRecomendada = descricao;
            DistanciaKm = distanciaKm;
            DuracaoMinutos = duracaoMinutos;
            RiscoMaximo = riscoMaximo;
        }

        public void RegistrarSemCaminhoSeco(int avaliadas)
        {
            OpcoesAvaliadas = avaliadas;
            OpcoesBloqueadas = avaliadas;
            SemCaminhoSeco = true;
        }

        public override void Validar()
        {
            if (UsuarioId <= 0)
                throw new DomainException("Usuário inválido.");
            if (!Enum.IsDefined(Modo))
                throw new DomainException("Modo de deslocamento inválido.");

            var origem = new Coordenada(OrigemLatitude, OrigemLongitude);
            var destino = new Coordenada(DestinoLatitude, DestinoLongitude);
            if (origem.DistanciaKmAte(destino) < 0.05)
                throw new DomainException("Origem e destino precisam ser lugares diferentes.");
        }
    }
}
