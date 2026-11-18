using Microsoft.AspNetCore.Mvc;
using RotaSeca.DTOs;
using RotaSeca.Services;

namespace RotaSeca.Controllers
{
    [ApiController]
    [Route("rotas")]
    [Produces("application/json")]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _service;

        public RotasController(IRotaService service) => _service = service;

        [HttpPost]
        [EndpointSummary("Calcula até 3 rotas com tempo e % de alagamento, conforme o modo")]
        [ProducesResponseType<RotaCalculadaResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Calcular([FromBody] CalcularRotaRequest request, CancellationToken ct) =>
            Ok(await _service.CalcularAsync(request, ct));
    }
}
