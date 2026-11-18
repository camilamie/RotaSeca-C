using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RotaSeca.DTOs;
using RotaSeca.Services;

namespace RotaSeca.Controllers
{
    [ApiController]
    [Route("alertas")]
    [Produces("application/json")]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertaService _service;

        public AlertasController(IAlertaService service) => _service = service;

        [HttpGet]
        [EndpointSummary("Lista os alertas do usuário")]
        [ProducesResponseType<IReadOnlyList<AlertaResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery, BindRequired] int usuarioId,
                                                [FromQuery] bool apenasNaoLidos = false,
                                                CancellationToken ct = default) =>
            Ok(await _service.ListarAsync(usuarioId, apenasNaoLidos, ct));

        [HttpPost("gerar")]
        [EndpointSummary("Força a verificação dos lugares do usuário (o job faz isso a cada 15 min)")]
        [ProducesResponseType<AlertasGeradosResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Gerar([FromQuery, BindRequired] int usuarioId, CancellationToken ct) =>
            Ok(new AlertasGeradosResponse(await _service.GerarAlertasAsync(usuarioId, ct)));

        [HttpPatch("{id:int}/lido")]
        [EndpointSummary("Marca um alerta como lido")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarcarComoLido(int id, CancellationToken ct)
        {
            await _service.MarcarComoLidoAsync(id, ct);
            return NoContent();
        }
    }
}
