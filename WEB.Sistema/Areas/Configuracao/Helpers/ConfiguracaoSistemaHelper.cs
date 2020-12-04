using System.Web.Mvc;

namespace WEB.Areas.Configuracao.Helpers {

    public class ConfiguracaoSistemaHelper {

        //Atributos
        private static ConfiguracaoSistemaHelper _instance;

        //Propriedades
        public static ConfiguracaoSistemaHelper getInstance => _instance = _instance ?? new ConfiguracaoSistemaHelper();

        //
        public SelectList selectListTema(string selected) {
            var list = new[] { 
                    new{value = "skin-black", text = "Black Dark"},
                    new{value = "skin-black-light", text = "Black Light"},
                    new{value = "skin-blue", text = "Blue Dark"},
                    new{value = "skin-blue-light", text = "Blue Light"},
                    new{value = "skin-green", text = "Green Dark"},
                    new{value = "skin-green-light", text = "Green Light"},
                    new{value = "skin-purple", text = "Purple Dark"},
                    new{value = "skin-purple-light", text = "Purple Light"},
                    new{value = "skin-red", text = "Red Dark"},
                    new{value = "skin-red-light", text = "Red Light"},
                    new{value = "skin-yellow", text = "Yellow Dark"},
                    new{value = "skin-yellow-light", text = "Yellow Light"},
            };
            return new SelectList(list, "value", "text", selected);
        }


    }
}