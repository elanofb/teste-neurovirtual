using System.Web.Mvc;

namespace WEB.Helpers {
    public class EscolaridadeHelper {

        public static SelectList getEscolaridade(int? selected) {
                var escolaridade = new[]{
                    new {value = "Ensino Fundamental", text = "Ensino Fundamental"},
                    new {value = "Ensino Medio", text = "Ensino Medio"},
                    new {value = "Ensino Superior", text = "Ensino Superior"},
                    new {value = "Pós Graduação", text = "Pós Graduação"},
                    new {value = "PHD", text = "PHD"}
                };
            return new SelectList(escolaridade, "value", "text", selected);
        }
    }
}