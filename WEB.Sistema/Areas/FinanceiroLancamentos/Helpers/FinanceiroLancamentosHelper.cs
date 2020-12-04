using System.Web.Mvc;

namespace WEB.Areas.FinanceiroLancamentos.Helpers {

    public class FinanceiroLancamentosHelper {
        
        public static SelectList getDtPesquisaFinanceiroExtrato(string selected) {
            var list = new[] {
                new{value = "V", text = "Data Vencimento"},
                new{value = "P", text = "Data Pagamento"},
                new{value = "PC", text = "Previsão de Crédito"},
                new{value = "C", text = "Data de Crédito"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getDtPesquisaFinanceiroConciliacao(string selected) {
            
            var list = new[] {                
                new{value = "PC", text = "Previsão de Crédito"},
                new{value = "P", text = "Data Pagamento"},
                new{value = "V", text = "Data Vencimento"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getDtPesquisaFinanceiroReceberExcluidas(string selected) {
            var list = new[] {
                new{value = "V", text = "Data Vencimento"},
                new{value = "P", text = "Data Pagamento / Operação"},
                new{value = "C", text = "Data de Crédito"},
                new{value = "E", text = "Data Exclusão"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getDtPesquisaFinanceiroPagar(string selected) {
            var list = new[] {
                new{value = "V", text = "Data Vencimento"},
                new{value = "P", text = "Data Pagamento / Operação"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getDtPesquisaFinanceiroPagarExcluidas(string selected) {
            var list = new[] {
                new{value = "V", text = "Data Vencimento"},
                new{value = "P", text = "Data Pagamento / Operação"},
                new{value = "E", text = "Data Exclusão"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getSituacao(string selected) {
            var list = new[] {
                    new{value = "", text = "Todos"},
                    new{value = "S", text = "Pago"},
                    new{value = "N", text = "Não Pago"},
                    new{value = "A", text = "Em atraso"},
            };

            return new SelectList(list, "value", "text", selected);
        }
    }
}
