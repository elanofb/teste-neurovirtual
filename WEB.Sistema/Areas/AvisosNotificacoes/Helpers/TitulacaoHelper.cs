using System.Web.Mvc;

namespace WEB.Areas.AvisosNotificacoes.Helpers{

	//
    public class AlvoEnvioHelper {

		// Atributos
        private static AlvoEnvioHelper _instance;
        
        // Propriedades
        public static AlvoEnvioHelper getInstance => _instance = _instance ?? new AlvoEnvioHelper(); 

        //
        public SelectList selectList(string selected){
            
            var list = new[] {
                new { value = "app", text = "APP" },
                new { value = "email", text = "E-mail" }
            };
            
            return new SelectList(list, "value", "text", selected);
        }


    }
}