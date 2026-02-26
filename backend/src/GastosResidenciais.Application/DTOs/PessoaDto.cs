using System.ComponentModel.DataAnnotations;

namespace GastosResidenciais.Application.DTOs
{
    // DTO para criacao e edicao de pessoa
    public class PessoaInputDto
    {
        [Required(ErrorMessage = "Nome eh obrigatorio")]
        [MaxLength(200, ErrorMessage = "Nome deve ter no maximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Idade eh obrigatoria")]
        [Range(0, 150, ErrorMessage = "Idade deve ser entre 0 e 150")]
        public int Idade { get; set; }
    }

    // DTO para retorno de pessoa
    public class PessoaOutputDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
    }

    // DTO para totais por pessoa
    public class PessoaTotaisDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; }
    }

    // DTO para totais gerais
    public class TotaisGeraisDto
    {
        public List<PessoaTotaisDto> Pessoas { get; set; } = new();
        public decimal TotalGeralReceitas { get; set; }
        public decimal TotalGeralDespesas { get; set; }
        public decimal SaldoLiquido { get; set; }
    }
}
