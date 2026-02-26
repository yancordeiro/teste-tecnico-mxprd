using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<IEnumerable<TransacaoOutputDto>> ObterTodosAsync();
        Task<TransacaoOutputDto?> ObterPorIdAsync(Guid id);
        Task<TransacaoOutputDto> CriarAsync(TransacaoInputDto dto);
    }
}
