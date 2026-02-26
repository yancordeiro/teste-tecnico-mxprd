using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;
using GastosResidenciais.Domain.Interfaces;

namespace GastosResidenciais.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public TransacaoService(
            ITransacaoRepository transacaoRepository,
            IPessoaRepository pessoaRepository,
            ICategoriaRepository categoriaRepository)
        {
            _transacaoRepository = transacaoRepository;
            _pessoaRepository = pessoaRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<TransacaoOutputDto>> ObterTodosAsync()
        {
            var transacoes = await _transacaoRepository.ObterTodosAsync();
            return transacoes.Select(MapToOutputDto);
        }

        public async Task<TransacaoOutputDto?> ObterPorIdAsync(Guid id)
        {
            var transacao = await _transacaoRepository.ObterPorIdAsync(id);
            return transacao == null ? null : MapToOutputDto(transacao);
        }

        public async Task<TransacaoOutputDto> CriarAsync(TransacaoInputDto dto)
        {
            // Valida se a pessoa existe
            var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId);
            if (pessoa == null)
                throw new ArgumentException("Pessoa nao encontrada");

            // Valida se a categoria existe
            var categoria = await _categoriaRepository.ObterPorIdAsync(dto.CategoriaId);
            if (categoria == null)
                throw new ArgumentException("Categoria nao encontrada");

            // Regra: menor de idade so pode ter despesas
            if (pessoa.EhMenorDeIdade() && dto.Tipo == Finalidade.Receita)
                throw new ArgumentException("Menores de idade so podem registrar despesas");

            // Regra: categoria deve aceitar o tipo de transacao
            // Ex: se tipo eh despesa, categoria nao pode ser somente receita
            if (!categoria.AceitaTipoTransacao(dto.Tipo))
                throw new ArgumentException($"Categoria '{categoria.Descricao}' nao aceita transacoes do tipo {dto.Tipo}");

            var transacao = new Transacao
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                CategoriaId = dto.CategoriaId,
                PessoaId = dto.PessoaId,
                Categoria = categoria,
                Pessoa = pessoa
            };

            var transacaoCriada = await _transacaoRepository.AdicionarAsync(transacao);

            // Recarrega para ter os dados de navegacao
            transacaoCriada.Categoria = categoria;
            transacaoCriada.Pessoa = pessoa;

            return MapToOutputDto(transacaoCriada);
        }

        private static TransacaoOutputDto MapToOutputDto(Transacao transacao)
        {
            return new TransacaoOutputDto
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                TipoDescricao = transacao.Tipo.ToString(),
                CategoriaId = transacao.CategoriaId,
                CategoriaDescricao = transacao.Categoria?.Descricao ?? string.Empty,
                PessoaId = transacao.PessoaId,
                PessoaNome = transacao.Pessoa?.Nome ?? string.Empty
            };
        }
    }
}
