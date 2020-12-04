namespace WEB.Areas.Financeiro.ViewModels {

    public class FluxoCaixaEvolucaoDTO {

        public string[] datas { get; set; }

        public decimal[] valoresReceitas { get; set; }

        public decimal[] valoresDespesas { get; set; }

        public decimal[] saldosAcumulados { get; set; }

    }

}