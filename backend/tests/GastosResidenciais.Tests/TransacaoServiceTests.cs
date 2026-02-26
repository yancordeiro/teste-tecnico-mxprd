using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Services;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;
using GastosResidenciais.Domain.Interfaces;
using Moq;

namespace GastosResidenciais.Tests
{
    public class TransacaoServiceTests
    {
        private readonly Mock<ITransacaoRepository> _transacaoRepoMock;
        private readonly Mock<IPessoaRepository> _pessoaRepoMock;
        private readonly Mock<ICategoriaRepository> _categoriaRepoMock;
        private readonly TransacaoService _service;

        public TransacaoServiceTests()
        {
            _transacaoRepoMock = new Mock<ITransacaoRepository>();
            _pessoaRepoMock = new Mock<IPessoaRepository>();
            _categoriaRepoMock = new Mock<ICategoriaRepository>();
            _service = new TransacaoService(_transacaoRepoMock.Object, _pessoaRepoMock.Object, _categoriaRepoMock.Object);
        }

        [Fact]
        public async Task Criar_MenorDeIdadeComReceita_DeveLancarExcecao()
        {
            // Arrange
            var pessoaId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();
            var pessoa = new Pessoa { Id = pessoaId, Nome = "Menor", Idade = 15 };
            var categoria = new Categoria { Id = categoriaId, Descricao = "Geral", Finalidade = Finalidade.Ambas };
            var dto = new TransacaoInputDto
            {
                Descricao = "Receita teste",
                Valor = 100,
                Tipo = Finalidade.Receita,
                PessoaId = pessoaId,
                CategoriaId = categoriaId
            };

            _pessoaRepoMock.Setup(r => r.ObterPorIdAsync(pessoaId)).ReturnsAsync(pessoa);
            _categoriaRepoMock.Setup(r => r.ObterPorIdAsync(categoriaId)).ReturnsAsync(categoria);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
            Assert.Equal("Menores de idade so podem registrar despesas", exception.Message);
        }

        [Fact]
        public async Task Criar_MenorDeIdadeComDespesa_DeveFuncionar()
        {
            // Arrange
            var pessoaId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();
            var pessoa = new Pessoa { Id = pessoaId, Nome = "Menor", Idade = 15 };
            var categoria = new Categoria { Id = categoriaId, Descricao = "Alimentacao", Finalidade = Finalidade.Despesa };
            var dto = new TransacaoInputDto
            {
                Descricao = "Despesa teste",
                Valor = 50,
                Tipo = Finalidade.Despesa,
                PessoaId = pessoaId,
                CategoriaId = categoriaId
            };

            _pessoaRepoMock.Setup(r => r.ObterPorIdAsync(pessoaId)).ReturnsAsync(pessoa);
            _categoriaRepoMock.Setup(r => r.ObterPorIdAsync(categoriaId)).ReturnsAsync(categoria);
            _transacaoRepoMock.Setup(r => r.AdicionarAsync(It.IsAny<Transacao>()))
                .ReturnsAsync((Transacao t) => t);

            // Act
            var resultado = await _service.CriarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Descricao, resultado.Descricao);
        }

        [Fact]
        public async Task Criar_CategoriaIncompativelComTipo_DeveLancarExcecao()
        {
            // Arrange
            var pessoaId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();
            var pessoa = new Pessoa { Id = pessoaId, Nome = "Adulto", Idade = 30 };
            var categoria = new Categoria { Id = categoriaId, Descricao = "Salario", Finalidade = Finalidade.Receita };
            var dto = new TransacaoInputDto
            {
                Descricao = "Despesa invalida",
                Valor = 100,
                Tipo = Finalidade.Despesa, // Tentando usar categoria de receita para despesa
                PessoaId = pessoaId,
                CategoriaId = categoriaId
            };

            _pessoaRepoMock.Setup(r => r.ObterPorIdAsync(pessoaId)).ReturnsAsync(pessoa);
            _categoriaRepoMock.Setup(r => r.ObterPorIdAsync(categoriaId)).ReturnsAsync(categoria);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
            Assert.Contains("nao aceita transacoes do tipo", exception.Message);
        }

        [Fact]
        public async Task Criar_PessoaNaoEncontrada_DeveLancarExcecao()
        {
            // Arrange
            var pessoaId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();
            var dto = new TransacaoInputDto
            {
                Descricao = "Transacao",
                Valor = 100,
                Tipo = Finalidade.Despesa,
                PessoaId = pessoaId,
                CategoriaId = categoriaId
            };

            _pessoaRepoMock.Setup(r => r.ObterPorIdAsync(pessoaId)).ReturnsAsync((Pessoa?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
            Assert.Equal("Pessoa nao encontrada", exception.Message);
        }

        [Fact]
        public async Task Criar_CategoriaNaoEncontrada_DeveLancarExcecao()
        {
            // Arrange
            var pessoaId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();
            var pessoa = new Pessoa { Id = pessoaId, Nome = "Teste", Idade = 25 };
            var dto = new TransacaoInputDto
            {
                Descricao = "Transacao",
                Valor = 100,
                Tipo = Finalidade.Despesa,
                PessoaId = pessoaId,
                CategoriaId = categoriaId
            };

            _pessoaRepoMock.Setup(r => r.ObterPorIdAsync(pessoaId)).ReturnsAsync(pessoa);
            _categoriaRepoMock.Setup(r => r.ObterPorIdAsync(categoriaId)).ReturnsAsync((Categoria?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
            Assert.Equal("Categoria nao encontrada", exception.Message);
        }
    }
}
