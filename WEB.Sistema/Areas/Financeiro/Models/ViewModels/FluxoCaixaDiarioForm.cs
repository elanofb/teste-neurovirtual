using System;
using System.Web.Mvc;

namespace WEB.Areas.Financeiro.ViewModels {

    public class FluxoCaixaDiarioForm {
        
        public int? idContaBancaria { get; set; }

        public string tipoBuscaPeriodo { get; set; }

        public DateTime? dtInicioPeriodo { get; set; }

        public DateTime? dtFimPeriodo { get; set; }

        public SelectList selectListTipoPeriodo(string selected) {
            
            var lista = new[] {
                new { value = "dtPagamento", text = "Data de Pagamento" },  
                new { value = "dtCredito", text = "Data de Crédito" },  
                new { value = "dtPrevisaoCredito", text = "Data de Previsão de Crédito" },  
            };
            
            return new SelectList(lista, "value", "text", selected);
        }
    }
}
