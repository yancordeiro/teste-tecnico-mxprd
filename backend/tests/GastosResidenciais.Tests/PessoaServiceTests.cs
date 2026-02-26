using GastosResidenciais.Application.Services;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;
using GastosResidenciais.Domain.Interfaces;
using Moq;

namespace GastosResidenciais.Tests
{
    public class PessoaServiceTests
    {
        private readonly Mock<IPessoaRepository> _pessoaRepoMock;
        private readonly Mock<ITransacaoRepository> _transacaoRepoMock;
        private readonly PessoaService _service;

        public PessoaServiceTests()
        {
            _pessoaRepoMock = new Mock<IPessoaRepository>();
            _transacaoRepoMock = new Mock<ITransacaoRepository>();
            _service = new PessoaService(_pessoaRepoMock.Object, _transacaoRepoMock.Object);
        }

        [Fact]
        public async Task ObterTotaisPorPessoa_DeveCalcularCorretamente()
        {
            // Arrange
            var joaoId = Guid.NewGuid();
            var mariaId = Guid.NewGuid();

            var pessoas = new List<Pessoa>
            {
                new Pessoa { Id = joaoId, Nome = "Joao", Idade = 30 },
                new Pessoa { Id = mariaId, Nome = "Maria", Idade = 25 }
            };

            var transacoes = new List<Transacao>
            {
                new Transacao { Id = Guid.NewGuid(), PessoaId = joaoId, Tipo = Finalidade.Receita, Valor = 1000 },
                new Transacao { Id = Guid.NewGuid(), PessoaId = joaoId, Tipo = Finalidade.Despesa, Valor = 300 },
                new Transacao { Id = Guid.NewGuid(), PessoaId = mariaId, Tipo = Finalidade.Receita, Valor = 2000 },
                new Transacao { Id = Guid.NewGuid(), PessoaId = mariaId, Tipo = Finalidade.Despesa, Valor = 500 }
            };

            _pessoaRepoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(pessoas);
            _transacaoRepoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(transacoes);

            // Act
            var resultado = await _service.ObterTotaisPorPessoaAsync();

            // Assert
            Assert.Equal(2, resultado.Pessoas.Count);

            // Verifica totais do Joao
            var joao = resultado.Pessoas.First(p => p.Id == joaoId);
            Assert.Equal(1000, joao.TotalReceitas);
            Assert.Equal(300, joao.TotalDespesas);
            Assert.Equal(700, joao.Saldo); // 1000 - 300

            // Verifica totais da Maria
            var maria = resultado.Pessoas.First(p => p.Id == mariaId);
            Assert.Equal(2000, maria.TotalReceitas);
            Assert.Equal(500, maria.TotalDespesas);
            Assert.Equal(1500, maria.Saldo); // 2000 - 500

            // Verifica totais gerais
            Assert.Equal(3000, resultado.TotalGeralReceitas); // 1000 + 2000
            Assert.Equal(800, resultado.TotalGeralDespesas); // 300 + 500
            Assert.Equal(2200, resultado.SaldoLiquido); // 3000 - 800
        }

        [Fact]
        public async Task Remover_DeveRemoverTransacoesAntes()
        {
            // Arrange
            var pessoaId = Guid.NewGuid();
            var pessoa = new Pessoa { Id = pessoaId, Nome = "Teste", Idade = 25 };
            _pessoaRepoMock.Setup(r => r.ObterPorIdAsync(pessoaId)).ReturnsAsync(pessoa);

            // Act
            await _service.RemoverAsync(pessoaId);

            // Assert - verifica se as transacoes foram removidas antes da pessoa
            _transacaoRepoMock.Verify(r => r.RemoverPorPessoaIdAsync(pessoaId), Times.Once);
            _pessoaRepoMock.Verify(r => r.RemoverAsync(pessoaId), Times.Once);
        }
    }
}
