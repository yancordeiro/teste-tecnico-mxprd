namespace GastosResidenciais.Application.Responses
{
    public class CategoriaResponse
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int FinalidadeId { get; set; }
        public string FinalidadeDescricao { get; set; } = string.Empty;
    }

    public class CategoriaTotaisResponse
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; }
    }

    public class TotaisCategoriasResponse
    {
        public List<CategoriaTotaisResponse> Categorias { get; set; } = new();
        public decimal TotalGeralReceitas { get; set; }
        public decimal TotalGeralDespesas { get; set; }
        public decimal SaldoLiquido { get; set; }
    }
}
