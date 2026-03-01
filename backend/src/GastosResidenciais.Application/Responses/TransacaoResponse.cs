using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Responses
{
    public class TransacaoResponse
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
