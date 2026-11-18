using RotaSeca.Domain.Entities;

namespace RotaSeca.Repositories
{
    public interface IRepository<T> where T : EntidadeBase
    {
        Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default);
        Task AdicionarAsync(T entidade, CancellationToken ct = default);
        void Remover(T entidade);
        Task<int> SalvarAsync(CancellationToken ct = default);
    }
}
