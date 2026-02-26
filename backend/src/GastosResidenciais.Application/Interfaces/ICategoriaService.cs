using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaOutputDto>> ObterTodosAsync();
        Task<CategoriaOutputDto?> ObterPorIdAsync(Guid id);
        Task<CategoriaOutputDto> CriarAsync(CategoriaInputDto dto);
        Task<TotaisCategoriasDto> ObterTotaisPorCategoriaAsync();
    }
}
