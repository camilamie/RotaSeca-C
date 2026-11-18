using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Integrations
{
    public sealed record ChuvaAtual(double IntensidadeMmH, double PrevisaoProximaHoraMmH,
                                DateTime MedidoEmUtc, string Fonte);

    public sealed record RotaCandidata(string Descricao, double DistanciaKm,
                                       IReadOnlyList<Coordenada> Pontos);
}
