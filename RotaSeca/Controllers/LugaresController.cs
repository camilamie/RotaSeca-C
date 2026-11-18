using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RotaSeca.DTOs;
using RotaSeca.Services;

namespace RotaSeca.Controllers
{
    [ApiController]
    [Route("lugares")]
    [Produces("application/json")]
    public class LugaresController : ControllerBase
    {
        private readonly ILugarService _service;

        public LugaresController(ILugarService service) => _service = service;

        [HttpPost]
        [EndpointSummary("Salva um lugar (Casa, Trabalho, Faculdade...)")]
        [ProducesResponseType<LugarResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Criar([FromBody] SalvarLugarRequest request, CancellationToken ct)
        {
            var lugar = await _service.CriarAsync(request, ct);
            return CreatedAtAction(nameof(Listar), new { usuarioId = request.UsuarioId }, lugar);
        }

        [HttpGet]
        [EndpointSummary("Lista os lugares do usuário com o risco atual e a cor no mapa")]
        [ProducesResponseType<IReadOnlyList<LugarResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery, BindRequired] int usuarioId, CancellationToken ct) =>
            Ok(await _service.ListarAsync(usuarioId, ct));

        [HttpGet("em-risco")]
        [EndpointSummary("Lugares do usuário com risco acima do limite na próxima hora")]
        [ProducesResponseType<IReadOnlyList<LugarResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarEmRisco([FromQuery, BindRequired] int usuarioId,
                                                       [FromQuery] double limite = 0.5,
                                                       CancellationToken ct = default) =>
            Ok(await _service.ListarEmRiscoAsync(usuarioId, limite, ct));

        [HttpPut("{id:int}")]
        [EndpointSummary("Atualiza um lugar salvo")]
        [ProducesResponseType<LugarResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] SalvarLugarRequest request, CancellationToken ct) =>
            Ok(await _service.AtualizarAsync(id, request, ct));

        [HttpDelete("{id:int}")]
        [EndpointSummary("Remove um lugar salvo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(int id, CancellationToken ct)
        {
            await _service.RemoverAsync(id, ct);
            return NoContent();
        }
    }
}
