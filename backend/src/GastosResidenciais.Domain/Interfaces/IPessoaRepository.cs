using GastosResidenciais.Domain.Entities;

namespace GastosResidenciais.Domain.Interfaces
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<Pessoa>> ObterTodosAsync();
        Task<Pessoa?> ObterPorIdAsync(Guid id);
        Task<Pessoa> AdicionarAsync(Pessoa pessoa);
        Task AtualizarAsync(Pessoa pessoa);
        Task RemoverAsync(Guid id);
    }
}
