using Microsoft.AspNetCore.Mvc;
using RotaSeca.DTOs;
using RotaSeca.Services;

namespace RotaSeca.Controllers
{
    [ApiController]
    [Route("usuarios")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service) => _service = service;

        [HttpPost]
        [EndpointSummary("Cria um usuário (dados simulados)")]
        [ProducesResponseType<UsuarioResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request, CancellationToken ct)
        {
            var usuario = await _service.CriarAsync(request, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
        }

        [HttpGet("{id:int}")]
        [EndpointSummary("Consulta um usuário")]
        [ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id, CancellationToken ct) =>
            Ok(await _service.ObterAsync(id, ct));
    }
}
