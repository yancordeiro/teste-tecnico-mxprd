using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;
using GastosResidenciais.Domain.Interfaces;

namespace GastosResidenciais.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository, ITransacaoRepository transacaoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IEnumerable<CategoriaResponse>> ObterTodosAsync()
        {
            var categorias = await _categoriaRepository.ObterTodosAsync();
            return categorias.Select(MapToResponse);
        }

        public async Task<CategoriaResponse?> ObterPorIdAsync(Guid id)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);
            return categoria == null ? null : MapToResponse(categoria);
        }

        public async Task<CategoriaResponse> CriarAsync(CreateCategoriaRequest request)
        {
            var categoria = new Categoria
            {
                Descricao = request.Descricao,
                Finalidade = request.Finalidade
            };

            var categoriaCriada = await _categoriaRepository.AdicionarAsync(categoria);
            return MapToResponse(categoriaCriada);
        }

        // Retorna os totais de receitas, despesas e saldo por categoria (funcionalidade opcional)
        public async Task<TotaisCategoriasResponse> ObterTotaisPorCategoriaAsync()
        {
            var categorias = await _categoriaRepository.ObterTodosAsync();
            var transacoes = await _transacaoRepository.ObterTodosAsync();

            var categoriasTotais = categorias.Select(c =>
            {
                var transacoesCategoria = transacoes.Where(t => t.CategoriaId == c.Id);
                var totalReceitas = transacoesCategoria.Where(t => t.Tipo == Finalidade.Receita).Sum(t => t.Valor);
                var totalDespesas = transacoesCategoria.Where(t => t.Tipo == Finalidade.Despesa).Sum(t => t.Valor);

                return new CategoriaTotaisResponse
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            }).ToList();

            return new TotaisCategoriasResponse
            {
                Categorias = categoriasTotais,
                TotalGeralReceitas = categoriasTotais.Sum(c => c.TotalReceitas),
                TotalGeralDespesas = categoriasTotais.Sum(c => c.TotalDespesas),
                SaldoLiquido = categoriasTotais.Sum(c => c.Saldo)
            };
        }

        private static CategoriaResponse MapToResponse(Categoria categoria)
        {
            return new CategoriaResponse
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                FinalidadeId = (int)categoria.Finalidade,
                FinalidadeDescricao = categoria.Finalidade.ToString()
            };
        }
    }
}
