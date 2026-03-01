using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;

namespace GastosResidenciais.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<IEnumerable<TransacaoResponse>> ObterTodosAsync();
        Task<TransacaoResponse?> ObterPorIdAsync(Guid id);
        Task<TransacaoResponse> CriarAsync(CreateTransacaoRequest request);
    }
}
