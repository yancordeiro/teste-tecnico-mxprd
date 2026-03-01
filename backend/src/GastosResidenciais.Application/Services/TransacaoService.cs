using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;
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

        public async Task<IEnumerable<TransacaoResponse>> ObterTodosAsync()
        {
            var transacoes = await _transacaoRepository.ObterTodosAsync();
            return transacoes.Select(MapToResponse);
        }

        public async Task<TransacaoResponse?> ObterPorIdAsync(Guid id)
        {
            var transacao = await _transacaoRepository.ObterPorIdAsync(id);
            return transacao == null ? null : MapToResponse(transacao);
        }

        public async Task<TransacaoResponse> CriarAsync(CreateTransacaoRequest request)
        {
            // Valida se a pessoa existe
            var pessoa = await _pessoaRepository.ObterPorIdAsync(request.PessoaId);
            if (pessoa == null)
                throw new ArgumentException("Pessoa nao encontrada");

            // Valida se a categoria existe
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new ArgumentException("Categoria nao encontrada");

            // Regra: menor de idade so pode ter despesas
            if (pessoa.EhMenorDeIdade() && request.Tipo == Finalidade.Receita)
                throw new ArgumentException("Menores de idade so podem registrar despesas");

            // Regra: categoria deve aceitar o tipo de transacao
            // Ex: se tipo eh despesa, categoria nao pode ser somente receita
            if (!categoria.AceitaTipoTransacao(request.Tipo))
                throw new ArgumentException($"Categoria '{categoria.Descricao}' nao aceita transacoes do tipo {request.Tipo}");

            var transacao = new Transacao
            {
                Descricao = request.Descricao,
                Valor = request.Valor,
                Tipo = request.Tipo,
                CategoriaId = request.CategoriaId,
                PessoaId = request.PessoaId,
                Categoria = categoria,
                Pessoa = pessoa
            };

            var transacaoCriada = await _transacaoRepository.AdicionarAsync(transacao);

            // Recarrega para ter os dados de navegacao
            transacaoCriada.Categoria = categoria;
            transacaoCriada.Pessoa = pessoa;

            return MapToResponse(transacaoCriada);
        }

        private static TransacaoResponse MapToResponse(Transacao transacao)
        {
            return new TransacaoResponse
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
