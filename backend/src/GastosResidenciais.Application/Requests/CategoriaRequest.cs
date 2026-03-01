using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Requests
{
    public class CreateCategoriaRequest
    {
        [Required(ErrorMessage = "Descricao obrigatoria")]
        [MaxLength(400, ErrorMessage = "Descricao deve ter no maximo 400 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Finalidade obrigatoria")]
        public Finalidade Finalidade { get; set; }
    }
}
