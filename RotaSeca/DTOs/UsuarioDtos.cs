using RotaSeca.Domain.Enums;

namespace RotaSeca.DTOs
{
    public record CriarUsuarioRequest(string Nome, ModoDeslocamentoTipo ModoPreferido);

    public record UsuarioResponse(int Id, string Nome, ModoDeslocamentoTipo ModoPreferido, int TotalLugares);
}
