using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;
using GastosResidenciais.Domain.Interfaces;

namespace GastosResidenciais.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public PessoaService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
        {
            _pessoaRepository = pessoaRepository;
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IEnumerable<PessoaResponse>> ObterTodosAsync()
        {
            var pessoas = await _pessoaRepository.ObterTodosAsync();
            return pessoas.Select(MapToResponse);
        }

        public async Task<PessoaResponse?> ObterPorIdAsync(Guid id)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
            return pessoa == null ? null : MapToResponse(pessoa);
        }

        public async Task<PessoaResponse> CriarAsync(CreatePessoaRequest request)
        {
            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Idade = request.Idade
            };

            var pessoaCriada = await _pessoaRepository.AdicionarAsync(pessoa);
            return MapToResponse(pessoaCriada);
        }

        public async Task<PessoaResponse?> AtualizarAsync(Guid id, UpdatePessoaRequest request)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
            if (pessoa == null)
                return null;

            pessoa.Nome = request.Nome;
            pessoa.Idade = request.Idade;

            await _pessoaRepository.AtualizarAsync(pessoa);
            return MapToResponse(pessoa);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
            if (pessoa == null)
                return false;

            // Remove todas as transacoes da pessoa antes de remover a pessoa
            await _transacaoRepository.RemoverPorPessoaIdAsync(id);
            await _pessoaRepository.RemoverAsync(id);
            return true;
        }

        // Retorna os totais de receitas, despesas e saldo por pessoa
        public async Task<TotaisGeraisResponse> ObterTotaisPorPessoaAsync()
        {
            var pessoas = await _pessoaRepository.ObterTodosAsync();
            var transacoes = await _transacaoRepository.ObterTodosAsync();

            var pessoasTotais = pessoas.Select(p =>
            {
                var transacoesPessoa = transacoes.Where(t => t.PessoaId == p.Id);
                var totalReceitas = transacoesPessoa.Where(t => t.Tipo == Finalidade.Receita).Sum(t => t.Valor);
                var totalDespesas = transacoesPessoa.Where(t => t.Tipo == Finalidade.Despesa).Sum(t => t.Valor);

                return new PessoaTotaisResponse
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            }).ToList();

            return new TotaisGeraisResponse
            {
                Pessoas = pessoasTotais,
                TotalGeralReceitas = pessoasTotais.Sum(p => p.TotalReceitas),
                TotalGeralDespesas = pessoasTotais.Sum(p => p.TotalDespesas),
                SaldoLiquido = pessoasTotais.Sum(p => p.Saldo)
            };
        }

        private static PessoaResponse MapToResponse(Pessoa pessoa)
        {
            return new PessoaResponse
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };
        }
    }
}
