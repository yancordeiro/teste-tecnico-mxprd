using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;

namespace GastosResidenciais.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponse>> ObterTodosAsync();
        Task<CategoriaResponse?> ObterPorIdAsync(Guid id);
        Task<CategoriaResponse> CriarAsync(CreateCategoriaRequest request);
        Task<TotaisCategoriasResponse> ObterTotaisPorCategoriaAsync();
    }
}
