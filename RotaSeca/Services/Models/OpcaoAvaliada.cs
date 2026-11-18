using RotaSeca.Domain.ValueObjects;

namespace RotaSeca.Services.Models
{
    public sealed record OpcaoAvaliada(string Descricao, double DistanciaKm, int DuracaoMinutos,
                                   double RiscoMaximo, bool Bloqueada, IReadOnlyList<Coordenada> Trajeto);
}
