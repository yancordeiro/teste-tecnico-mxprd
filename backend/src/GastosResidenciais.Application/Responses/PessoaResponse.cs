namespace GastosResidenciais.Application.Responses
{
    public class PessoaResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
    }

    public class PessoaTotaisResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; }
    }

    public class TotaisGeraisResponse
    {
        public List<PessoaTotaisResponse> Pessoas { get; set; } = new();
        public decimal TotalGeralReceitas { get; set; }
        public decimal TotalGeralDespesas { get; set; }
        public decimal SaldoLiquido { get; set; }
    }
}
