namespace GastosResidenciais.Domain.Entities
{
    public class Pessoa
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }

        // Navegacao para as transacoes da pessoa
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

        // Verifica se a pessoa eh menor de idade
        public bool EhMenorDeIdade() => Idade < 18;
    }
}
