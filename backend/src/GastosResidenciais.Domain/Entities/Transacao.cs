using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Domain.Entities
{
    public class Transacao
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public Finalidade Tipo { get; set; }

        // Chaves estrangeiras
        public Guid CategoriaId { get; set; }
        public Guid PessoaId { get; set; }

        // Navegacao
        public Categoria Categoria { get; set; } = null!;
        public Pessoa Pessoa { get; set; } = null!;
    }
}
