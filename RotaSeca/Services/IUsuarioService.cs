using RotaSeca.DTOs;

namespace RotaSeca.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request, CancellationToken ct = default);
        Task<UsuarioResponse> ObterAsync(int id, CancellationToken ct = default);
    }
}
