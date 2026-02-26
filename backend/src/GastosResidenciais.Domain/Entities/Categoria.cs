using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Domain.Entities
{
    public class Categoria
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Descricao { get; set; } = string.Empty;
        public Finalidade Finalidade { get; set; }

        // Navegacao para as transacoes desta categoria
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

        // Verifica se a categoria aceita o tipo de transacao informado
        public bool AceitaTipoTransacao(Finalidade tipoTransacao)
        {
            if (Finalidade == Finalidade.Ambas)
                return true;

            return Finalidade == tipoTransacao;
        }
    }
}
