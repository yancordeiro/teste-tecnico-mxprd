using System.ComponentModel.DataAnnotations;

namespace GastosResidenciais.Application.Requests
{
    public class CreatePessoaRequest
    {
        [Required(ErrorMessage = "Nome eh obrigatorio")]
        [MaxLength(200, ErrorMessage = "Nome deve ter no maximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Idade eh obrigatoria")]
        [Range(0, 150, ErrorMessage = "Idade deve ser entre 0 e 150")]
        public int Idade { get; set; }
    }

    public class UpdatePessoaRequest
    {
        [Required(ErrorMessage = "Nome eh obrigatorio")]
        [MaxLength(200, ErrorMessage = "Nome deve ter no maximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Idade eh obrigatoria")]
        [Range(0, 150, ErrorMessage = "Idade deve ser entre 0 e 150")]
        public int Idade { get; set; }
    }
}
