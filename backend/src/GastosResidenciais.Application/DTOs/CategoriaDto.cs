using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.DTOs
{
    // DTO para criacao de categoria
    public class CategoriaInputDto
    {
        [Required(ErrorMessage = "Descricao obrigatoria")]
        [MaxLength(400, ErrorMessage = "Descricao deve ter no maximo 400 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Finalidade obrigatoria")]
        public Finalidade Finalidade { get; set; }
    }

    // DTO para retorno de categoria
    public class CategoriaOutputDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int FinalidadeId { get; set; }
        public string FinalidadeDescricao { get; set; } = string.Empty;
    }

    // DTO para totais por categoria (opcional)
    public class CategoriaTotaisDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; }
    }

    // DTO para totais gerais por categoria
    public class TotaisCategoriasDto
    {
        public List<CategoriaTotaisDto> Categorias { get; set; } = new();
        public decimal TotalGeralReceitas { get; set; }
        public decimal TotalGeralDespesas { get; set; }
        public decimal SaldoLiquido { get; set; }
    }
}
