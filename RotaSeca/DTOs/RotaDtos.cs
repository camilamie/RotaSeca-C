using RotaSeca.Domain.Enums;

namespace RotaSeca.DTOs
{
    public record CoordenadaDto(double Latitude, double Longitude);

    public record CalcularRotaRequest(int UsuarioId, CoordenadaDto Origem, CoordenadaDto Destino,
                                      ModoDeslocamentoTipo Modo);

    public record OpcaoRotaResponse(
        string Descricao,
        double DistanciaKm,
        int DuracaoMinutos,
        double RiscoMaximo,
        int PercentualAlagamento,
        bool Bloqueada,
        bool Recomendada,
        IReadOnlyList<CoordenadaDto> Trajeto);

    public record RotaCalculadaResponse(
        int ConsultaId,
        string Modo,
        double LimiteBloqueio,
        double ChuvaAtualMmH,
        bool SemCaminhoSeco,
        string Mensagem,
        IReadOnlyList<OpcaoRotaResponse> Opcoes);
}
