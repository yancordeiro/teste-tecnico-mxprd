using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Tests
{
    public class CategoriaTests
    {
        [Fact]
        public void AceitaTipoTransacao_CategoriaAmbas_AceitaReceita()
        {
            // Arrange
            var categoria = new Categoria { Descricao = "Geral", Finalidade = Finalidade.Ambas };

            // Act
            var resultado = categoria.AceitaTipoTransacao(Finalidade.Receita);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void AceitaTipoTransacao_CategoriaAmbas_AceitaDespesa()
        {
            // Arrange
            var categoria = new Categoria { Descricao = "Geral", Finalidade = Finalidade.Ambas };

            // Act
            var resultado = categoria.AceitaTipoTransacao(Finalidade.Despesa);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void AceitaTipoTransacao_CategoriaDespesa_AceitaDespesa()
        {
            // Arrange
            var categoria = new Categoria { Descricao = "Alimentacao", Finalidade = Finalidade.Despesa };

            // Act
            var resultado = categoria.AceitaTipoTransacao(Finalidade.Despesa);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void AceitaTipoTransacao_CategoriaDespesa_NaoAceitaReceita()
        {
            // Arrange
            var categoria = new Categoria { Descricao = "Alimentacao", Finalidade = Finalidade.Despesa };

            // Act
            var resultado = categoria.AceitaTipoTransacao(Finalidade.Receita);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void AceitaTipoTransacao_CategoriaReceita_AceitaReceita()
        {
            // Arrange
            var categoria = new Categoria { Descricao = "Salario", Finalidade = Finalidade.Receita };

            // Act
            var resultado = categoria.AceitaTipoTransacao(Finalidade.Receita);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void AceitaTipoTransacao_CategoriaReceita_NaoAceitaDespesa()
        {
            // Arrange
            var categoria = new Categoria { Descricao = "Salario", Finalidade = Finalidade.Receita };

            // Act
            var resultado = categoria.AceitaTipoTransacao(Finalidade.Despesa);

            // Assert
            Assert.False(resultado);
        }
    }
}
