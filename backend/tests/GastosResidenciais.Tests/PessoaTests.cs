using GastosResidenciais.Domain.Entities;

namespace GastosResidenciais.Tests
{
    public class PessoaTests
    {
        [Fact]
        public void EhMenorDeIdade_QuandoIdadeMenorQue18_RetornaTrue()
        {
            // Arrange
            var pessoa = new Pessoa { Nome = "Joao", Idade = 17 };

            // Act
            var resultado = pessoa.EhMenorDeIdade();

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void EhMenorDeIdade_QuandoIdadeIgual18_RetornaFalse()
        {
            // Arrange
            var pessoa = new Pessoa { Nome = "Maria", Idade = 18 };

            // Act
            var resultado = pessoa.EhMenorDeIdade();

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void EhMenorDeIdade_QuandoIdadeMaiorQue18_RetornaFalse()
        {
            // Arrange
            var pessoa = new Pessoa { Nome = "Pedro", Idade = 25 };

            // Act
            var resultado = pessoa.EhMenorDeIdade();

            // Assert
            Assert.False(resultado);
        }
    }
}
