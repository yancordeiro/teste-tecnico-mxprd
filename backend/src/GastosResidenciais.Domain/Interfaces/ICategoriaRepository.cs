using GastosResidenciais.Domain.Entities;

namespace GastosResidenciais.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ObterTodosAsync();
        Task<Categoria?> ObterPorIdAsync(Guid id);
        Task<Categoria> AdicionarAsync(Categoria categoria);
    }
}
