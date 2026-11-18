using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RotaSeca.Domain.Exceptions;

namespace RotaSeca.Middlewares
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
        {
            var (status, titulo) = exception switch
            {
                DomainException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
                NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
                HttpRequestException => (StatusCodes.Status503ServiceUnavailable, "Serviço de Risco da Rua indisponível"),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno")
            };

            if (status >= 500)
                _logger.LogError(exception, "Erro ao processar {Path}", context.Request.Path);

            var problema = new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = status == 500 ? "Ocorreu um erro inesperado." : exception.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problema, ct);
            return true;
        }
    }
}
