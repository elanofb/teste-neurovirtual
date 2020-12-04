namespace DAL.Financeiro.Procedures {

    public class SpContaBancariaSaldoAtual {

        public int id { get; set; }

        public string descricao { get; set; }

        public decimal valorTotalReceitas { get; set; }

        public decimal valorTotalDespesas { get; set; }

        public decimal saldoAtual { get; set; }

    }

}
