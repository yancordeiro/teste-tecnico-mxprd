using GastosResidenciais.Domain.Entities;

namespace GastosResidenciais.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<IEnumerable<Transacao>> ObterTodosAsync();
        Task<Transacao?> ObterPorIdAsync(Guid id);
        Task<Transacao> AdicionarAsync(Transacao transacao);
        Task RemoverPorPessoaIdAsync(Guid pessoaId);
    }
}
