using System.Web.Mvc;

namespace WEB.Areas.FinanceiroLancamentos.Helpers {

    public class TipoBaixarHelper {

        public static SelectList selectList(string selected) {

            var list = new[] {
                new{value = "M", text = "Manual"},
                new{value = "A", text = "Automática"}
            };

            return new SelectList(list, "value", "text", selected);
        }
    }
}
