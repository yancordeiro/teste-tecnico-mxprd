using GastosResidenciais.Application.DTOs;
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

        public async Task<IEnumerable<PessoaOutputDto>> ObterTodosAsync()
        {
            var pessoas = await _pessoaRepository.ObterTodosAsync();
            return pessoas.Select(MapToOutputDto);
        }

        public async Task<PessoaOutputDto?> ObterPorIdAsync(Guid id)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
            return pessoa == null ? null : MapToOutputDto(pessoa);
        }

        public async Task<PessoaOutputDto> CriarAsync(PessoaInputDto dto)
        {
            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Idade = dto.Idade
            };

            var pessoaCriada = await _pessoaRepository.AdicionarAsync(pessoa);
            return MapToOutputDto(pessoaCriada);
        }

        public async Task<PessoaOutputDto?> AtualizarAsync(Guid id, PessoaInputDto dto)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
            if (pessoa == null)
                return null;

            pessoa.Nome = dto.Nome;
            pessoa.Idade = dto.Idade;

            await _pessoaRepository.AtualizarAsync(pessoa);
            return MapToOutputDto(pessoa);
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
        public async Task<TotaisGeraisDto> ObterTotaisPorPessoaAsync()
        {
            var pessoas = await _pessoaRepository.ObterTodosAsync();
            var transacoes = await _transacaoRepository.ObterTodosAsync();

            var pessoasTotais = pessoas.Select(p =>
            {
                var transacoesPessoa = transacoes.Where(t => t.PessoaId == p.Id);
                var totalReceitas = transacoesPessoa.Where(t => t.Tipo == Finalidade.Receita).Sum(t => t.Valor);
                var totalDespesas = transacoesPessoa.Where(t => t.Tipo == Finalidade.Despesa).Sum(t => t.Valor);

                return new PessoaTotaisDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            }).ToList();

            return new TotaisGeraisDto
            {
                Pessoas = pessoasTotais,
                TotalGeralReceitas = pessoasTotais.Sum(p => p.TotalReceitas),
                TotalGeralDespesas = pessoasTotais.Sum(p => p.TotalDespesas),
                SaldoLiquido = pessoasTotais.Sum(p => p.Saldo)
            };
        }

        private static PessoaOutputDto MapToOutputDto(Pessoa pessoa)
        {
            return new PessoaOutputDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };
        }
    }
}
