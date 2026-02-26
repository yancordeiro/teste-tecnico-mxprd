using GastosResidenciais.Application.DTOs;
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

        public async Task<IEnumerable<CategoriaOutputDto>> ObterTodosAsync()
        {
            var categorias = await _categoriaRepository.ObterTodosAsync();
            return categorias.Select(MapToOutputDto);
        }

        public async Task<CategoriaOutputDto?> ObterPorIdAsync(Guid id)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);
            return categoria == null ? null : MapToOutputDto(categoria);
        }

        public async Task<CategoriaOutputDto> CriarAsync(CategoriaInputDto dto)
        {
            var categoria = new Categoria
            {
                Descricao = dto.Descricao,
                Finalidade = dto.Finalidade
            };

            var categoriaCriada = await _categoriaRepository.AdicionarAsync(categoria);
            return MapToOutputDto(categoriaCriada);
        }

        // Retorna os totais de receitas, despesas e saldo por categoria (funcionalidade opcional)
        public async Task<TotaisCategoriasDto> ObterTotaisPorCategoriaAsync()
        {
            var categorias = await _categoriaRepository.ObterTodosAsync();
            var transacoes = await _transacaoRepository.ObterTodosAsync();

            var categoriasTotais = categorias.Select(c =>
            {
                var transacoesCategoria = transacoes.Where(t => t.CategoriaId == c.Id);
                var totalReceitas = transacoesCategoria.Where(t => t.Tipo == Finalidade.Receita).Sum(t => t.Valor);
                var totalDespesas = transacoesCategoria.Where(t => t.Tipo == Finalidade.Despesa).Sum(t => t.Valor);

                return new CategoriaTotaisDto
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            }).ToList();

            return new TotaisCategoriasDto
            {
                Categorias = categoriasTotais,
                TotalGeralReceitas = categoriasTotais.Sum(c => c.TotalReceitas),
                TotalGeralDespesas = categoriasTotais.Sum(c => c.TotalDespesas),
                SaldoLiquido = categoriasTotais.Sum(c => c.Saldo)
            };
        }

        private static CategoriaOutputDto MapToOutputDto(Categoria categoria)
        {
            return new CategoriaOutputDto
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                FinalidadeId = (int)categoria.Finalidade,
                FinalidadeDescricao = categoria.Finalidade.ToString()
            };
        }
    }
}
