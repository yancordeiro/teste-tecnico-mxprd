using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.DTOs
{
    // DTO para criacao de transacao
    public class TransacaoInputDto
    {
        [Required(ErrorMessage = "Descricao eh obrigatoria")]
        [MaxLength(400, ErrorMessage = "Descricao deve ter no maximo 400 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor eh obrigatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser positivo")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Tipo eh obrigatorio")]
        public Finalidade Tipo { get; set; }

        [Required(ErrorMessage = "CategoriaId eh obrigatorio")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "PessoaId eh obrigatorio")]
        public Guid PessoaId { get; set; }
    }

    // DTO para retorno de transacao
    public class TransacaoOutputDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public Finalidade Tipo { get; set; }
        public string TipoDescricao { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }
        public string CategoriaDescricao { get; set; } = string.Empty;
        public Guid PessoaId { get; set; }
        public string PessoaNome { get; set; } = string.Empty;
    }
}
