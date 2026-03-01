using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;

namespace GastosResidenciais.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<PessoaResponse>> ObterTodosAsync();
        Task<PessoaResponse?> ObterPorIdAsync(Guid id);
        Task<PessoaResponse> CriarAsync(CreatePessoaRequest request);
        Task<PessoaResponse?> AtualizarAsync(Guid id, UpdatePessoaRequest request);
        Task<bool> RemoverAsync(Guid id);
        Task<TotaisGeraisResponse> ObterTotaisPorPessoaAsync();
    }
}
