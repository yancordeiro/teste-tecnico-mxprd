using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Requests
{
    public class CreateTransacaoRequest
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
}
