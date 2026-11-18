using Microsoft.AspNetCore.Mvc;
using RotaSeca.DTOs;
using RotaSeca.Services;

namespace RotaSeca.Controllers
{
    [ApiController]
    [Route("reportes")]
    [Produces("application/json")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _service;

        public ReportesController(IReporteService service) => _service = service;

        [HttpPost]
        [EndpointSummary("Registra um reporte de alagamento (reporte em dois toques)")]
        [ProducesResponseType<ReporteResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CriarReporteRequest request, CancellationToken ct)
        {
            var reporte = await _service.CriarAsync(request, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = reporte.Id }, reporte);
        }

        [HttpGet("{id:int}")]
        [EndpointSummary("Consulta um reporte")]
        [ProducesResponseType<ReporteResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id, CancellationToken ct) =>
            Ok(await _service.ObterAsync(id, ct));

        [HttpGet]
        [EndpointSummary("Lista os reportes ativos das últimas horas")]
        [ProducesResponseType<IReadOnlyList<ReporteResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarAtivos([FromQuery] int ultimasHoras = 6, CancellationToken ct = default) =>
            Ok(await _service.ListarAtivosAsync(ultimasHoras, ct));

        [HttpPost("{id:int}/confirmar")]
        [EndpointSummary("Outro usuário confirma que está alagado")]
        [ProducesResponseType<ReporteResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirmar(int id, CancellationToken ct) =>
            Ok(await _service.ConfirmarAsync(id, ct));

        [HttpPost("{id:int}/agua-baixou")]
        [EndpointSummary("Usuário avisa que a água baixou (2 avisos encerram o reporte)")]
        [ProducesResponseType<ReporteResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AguaBaixou(int id, CancellationToken ct) =>
            Ok(await _service.RegistrarAguaBaixouAsync(id, ct));
    }
}
