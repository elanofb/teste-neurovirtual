using System.Web.Mvc;

namespace WEB.Areas.PessoasDevices.Helpers{

    public class PlataformaHelper {

        // Atributos
        private static PlataformaHelper _instance;
        
        // Propriedades
        public static PlataformaHelper getInstance => _instance = _instance ?? new PlataformaHelper();

        // Constantes
        public static readonly string ANDROID = "android";
        public static readonly string IOS = "ios";
        
        //
        public SelectList selectList(string selected) {
            
            var list = new[] { 
                
                new { value = ANDROID, text = "Android" },
                new { value = IOS, text = "IOS" }
            };
            
            return new SelectList(list, "value", "text", selected);
            
        }

    }
}