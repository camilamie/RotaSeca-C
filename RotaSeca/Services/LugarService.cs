using RotaSeca.Domain.Entities;
using RotaSeca.Domain.Exceptions;
using RotaSeca.Domain.ValueObjects;
using RotaSeca.DTOs;
using RotaSeca.Integrations;
using RotaSeca.Repositories;
using RotaSeca.Mappings;

namespace RotaSeca.Services
{
    public class LugarService : ILugarService
    {
        private readonly ILugarRepository _lugares;
        private readonly IUsuarioRepository _usuarios;
        private readonly IRiscoProvider _risco;
        private readonly ILogger<LugarService> _logger;

        public LugarService(ILugarRepository lugares, IUsuarioRepository usuarios,
                            IRiscoProvider risco, ILogger<LugarService> logger)
        {
            _lugares = lugares;
            _usuarios = usuarios;
            _risco = risco;
            _logger = logger;
        }

        public async Task<LugarResponse> CriarAsync(SalvarLugarRequest request, CancellationToken ct = default)
        {
            await GarantirUsuarioAsync(request.UsuarioId, ct);

            if (await _lugares.ContarPorUsuarioAsync(request.UsuarioId, ct) >= LugarSalvo.MaximoPorUsuario)
                throw new DomainException($"Cada usuário pode salvar até {LugarSalvo.MaximoPorUsuario} lugares.");

            if (await _lugares.EtiquetaEmUsoAsync(request.UsuarioId, request.Etiqueta ?? string.Empty, null, ct))
                throw new DomainException($"Você já tem um lugar com a etiqueta '{request.Etiqueta}'.");

            var lugar = new LugarSalvo(request.UsuarioId, request.Etiqueta!, request.Endereco,
                                       new Coordenada(request.Latitude, request.Longitude),
                                       request.Dias, request.HorarioHabitual);

            lugar.RegistrarRisco(await _risco.ObterProbabilidadeAsync(lugar.ObterCoordenada(), DateTime.UtcNow, ct));

            await _lugares.AdicionarAsync(lugar, ct);
            await _lugares.SalvarAsync(ct);

            _logger.LogInformation("Lugar {LugarId} salvo para o usuário {UsuarioId}.", lugar.Id, lugar.UsuarioId);
            return lugar.ToResponse();
        }

        public async Task<IReadOnlyList<LugarResponse>> ListarAsync(int usuarioId, CancellationToken ct = default)
        {
            await GarantirUsuarioAsync(usuarioId, ct);
            var lugares = await _lugares.ListarPorUsuarioAsync(usuarioId, ct);

            await AtualizarRiscosAsync(lugares, DateTime.UtcNow, ct);
            await _lugares.SalvarAsync(ct);

            return lugares.Select(l => l.ToResponse()).ToList();
        }

        public async Task<IReadOnlyList<LugarResponse>> ListarEmRiscoAsync(int usuarioId, double limite, CancellationToken ct = default)
        {
            if (limite is < 0 or > 1)
                throw new DomainException("O limite deve estar entre 0 e 1 (ex.: 0.5 = 50%).");

            await GarantirUsuarioAsync(usuarioId, ct);

           
            var lugares = await _lugares.ListarPorUsuarioAsync(usuarioId, ct);
            await AtualizarRiscosAsync(lugares, DateTime.UtcNow.AddHours(1), ct);
            await _lugares.SalvarAsync(ct);

            
            var emRisco = await _lugares.ListarEmRiscoAsync(usuarioId, limite, ct);
            return emRisco.Select(l => l.ToResponse()).ToList();
        }

        public async Task<LugarResponse> AtualizarAsync(int id, SalvarLugarRequest request, CancellationToken ct = default)
        {
            var lugar = await _lugares.ObterPorIdAsync(id, ct)
                ?? throw new NotFoundException($"Lugar {id} não encontrado.");

            if (lugar.UsuarioId != request.UsuarioId)
                throw new DomainException("Este lugar pertence a outro usuário.");

            if (await _lugares.EtiquetaEmUsoAsync(request.UsuarioId, request.Etiqueta ?? string.Empty, id, ct))
                throw new DomainException($"Você já tem um lugar com a etiqueta '{request.Etiqueta}'.");

            lugar.Atualizar(request.Etiqueta!, request.Endereco, new Coordenada(request.Latitude, request.Longitude),
                            request.Dias, request.HorarioHabitual);
            lugar.RegistrarRisco(await _risco.ObterProbabilidadeAsync(lugar.ObterCoordenada(), DateTime.UtcNow, ct));

            await _lugares.SalvarAsync(ct);
            return lugar.ToResponse();
        }

        public async Task RemoverAsync(int id, CancellationToken ct = default)
        {
            var lugar = await _lugares.ObterPorIdAsync(id, ct)
                ?? throw new NotFoundException($"Lugar {id} não encontrado.");

            _lugares.Remover(lugar);
            await _lugares.SalvarAsync(ct);
        }

        private async Task AtualizarRiscosAsync(IEnumerable<LugarSalvo> lugares, DateTime momentoUtc, CancellationToken ct)
        {
            // Dispara todas as consultas ao mesmo tempo e espera todas terminarem
            var tarefas = lugares.Select(async l =>
                (Lugar: l, Risco: await _risco.ObterProbabilidadeAsync(l.ObterCoordenada(), momentoUtc, ct)));

            foreach (var (lugar, risco) in await Task.WhenAll(tarefas))
                lugar.RegistrarRisco(risco);
        }

        private async Task GarantirUsuarioAsync(int usuarioId, CancellationToken ct)
        {
            if (!await _usuarios.ExisteAsync(usuarioId, ct))
                throw new NotFoundException($"Usuário {usuarioId} não encontrado.");
        }
    }
}
