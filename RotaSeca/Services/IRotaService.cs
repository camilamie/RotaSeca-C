using RotaSeca.DTOs;

namespace RotaSeca.Services
{
    public interface IRotaService
    {
        Task<RotaCalculadaResponse> CalcularAsync(CalcularRotaRequest request, CancellationToken ct = default);
    }
}
