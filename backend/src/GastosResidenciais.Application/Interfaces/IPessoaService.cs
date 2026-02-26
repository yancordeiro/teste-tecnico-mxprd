using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<PessoaOutputDto>> ObterTodosAsync();
        Task<PessoaOutputDto?> ObterPorIdAsync(Guid id);
        Task<PessoaOutputDto> CriarAsync(PessoaInputDto dto);
        Task<PessoaOutputDto?> AtualizarAsync(Guid id, PessoaInputDto dto);
        Task<bool> RemoverAsync(Guid id);
        Task<TotaisGeraisDto> ObterTotaisPorPessoaAsync();
    }
}
