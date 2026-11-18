using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Exceptions;
using RotaSeca.DTOs;
using RotaSeca.Repositories;

namespace RotaSeca.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarios;
        private readonly ILugarRepository _lugares;

        public UsuarioService(IUsuarioRepository usuarios, ILugarRepository lugares)
        {
            _usuarios = usuarios;
            _lugares = lugares;
        }

        public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request, CancellationToken ct = default)
        {
            var usuario = new Usuario(request.Nome, request.ModoPreferido);
            await _usuarios.AdicionarAsync(usuario, ct);
            await _usuarios.SalvarAsync(ct);
            return usuario.ToResponse(totalLugares: 0);
        }

        public async Task<UsuarioResponse> ObterAsync(int id, CancellationToken ct = default)
        {
            var usuario = await _usuarios.ObterPorIdAsync(id, ct)
                ?? throw new NotFoundException($"Usuário {id} não encontrado.");
            var total = await _lugares.ContarPorUsuarioAsync(id, ct);
            return usuario.ToResponse(total);
        }
    }
}
