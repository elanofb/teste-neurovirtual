using System;
using System.Linq;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ResultadoFinanceiroTotalizadoresVM {
        
        public decimal valorTotalDespesas { get; set; }

        public decimal valorTotalReceitas { get; set; }

        public decimal valorResultado { get; set; }

        public decimal percentualResultado { get; set; }

        public bool? flagResultadoPositivo { get; set; }

        //
        public ResultadoFinanceiroTotalizadoresVM() {
            
        }
        
        //
        public void calcularTotais(ResultadoFinanceiroVM OResultadoFinanceiroVM) {
            
            this.valorTotalReceitas = OResultadoFinanceiroVM.listaPagamentos.Where(x => x.flagTipoTitulo.Equals("R") && x.valorRealizado > 0)
                                                                            .Sum(x => x.valorRealizado.toDecimal());
            
            this.valorTotalDespesas = OResultadoFinanceiroVM.listaPagamentos.Where(x => x.flagTipoTitulo.Equals("D")).Sum(x => x.valor);
            
            if (this.valorTotalReceitas > this.valorTotalDespesas) {

                this.flagResultadoPositivo = true;

                this.valorResultado = this.valorTotalReceitas - this.valorTotalDespesas;

                // Previnir divisão por 0 (zero).
                try {
                    this.percentualResultado = (this.valorResultado * 100) / this.valorTotalDespesas;
                } catch (Exception) {
                    this.percentualResultado = 0;
                }
            }
            
            if (this.valorTotalDespesas > this.valorTotalReceitas) {

                this.flagResultadoPositivo = false;

                this.valorResultado = this.valorTotalDespesas - this.valorTotalReceitas;
                
                // Previnir divisão por 0 (zero).
                try {
                    this.percentualResultado = (this.valorResultado * 100) / this.valorTotalReceitas;
                } catch (Exception) {
                    this.percentualResultado = 0;
                }

            }

            if (this.valorTotalReceitas == this.valorTotalDespesas) {

                this.valorResultado = 0;

                this.percentualResultado = 0;

            }

        }

    }
}
