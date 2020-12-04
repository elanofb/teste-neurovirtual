using System.Web.Mvc;

namespace WEB.Areas.ContasBancarias.Helpers {

    public class TipoContaHelper {
        //
        public static SelectList selectList(string selected) {
            var list = new[]{
                new {value = "C", text = "Corrente"},
                new {value = "P", text = "Poupança"}
            };

            return new SelectList(list, "value", "text", selected);
        }

    }
}