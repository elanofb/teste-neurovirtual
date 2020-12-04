using System.Web.Mvc;

namespace WEB.Areas.FinanceiroLancamentos.Helpers {

    public class RepeticaoPagamentoHelper {

        public static SelectList selectListRepeticao(string selected) {
            var list = new[] {
                new{value = "NENHUMA", text = "Nenhuma"},
                new{value = "PARCELAMENTO", text = "Parcelamento"},
                //new{value = "RECORRENTE", text = "Recorrente"}
            };

            return new SelectList(list, "value", "text", selected);
        }
    }
}
