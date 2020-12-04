using System.Web.Mvc;

namespace WEB.Helpers {
    public class TipoSaidaHelper {

        public static string HTML = "HTML";

        public static string EXCEL = "Excel";

        //
        public static SelectList selectListTipoSaida(string selected) {
            var list = new[] {
                    new { value = HTML, text = "Em Tela" },
                    new { value = EXCEL, text = EXCEL }
            };

            return new SelectList(list, "value", "text", selected);
        }

    }
}