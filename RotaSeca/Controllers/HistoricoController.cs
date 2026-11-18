using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RotaSeca.DTOs;
using RotaSeca.Services;

namespace RotaSeca.Controllers
{
    [ApiController]
    [Route("historico")]
    [Produces("application/json")]
    public class HistoricoController : ControllerBase
    {
        private readonly IHistoricoService _service;

        public HistoricoController(IHistoricoService service) => _service = service;

        [HttpGet]
        [EndpointSummary("Histórico do usuário e indicador de rotas que desviaram de risco")]
        [ProducesResponseType<HistoricoResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Obter([FromQuery, BindRequired] int usuarioId, CancellationToken ct) =>
            Ok(await _service.ObterAsync(usuarioId, ct));
    }
}
