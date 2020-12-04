using System.Web.Mvc;

namespace WEB.Areas.Financeiro.Helpers {
    
    public class TipoTituloHelper {
    
        //Atributos
        private static TipoTituloHelper _instance;
        
        //Propriedades
        public static TipoTituloHelper getInstance => _instance = _instance ?? new TipoTituloHelper();
        
        //
        public static SelectList getCombo(string selected) {
            
            var list = new[] { 
                    new{value = "1", text = "Despesa"},
                    new{value = "2", text = "Receita"},
            };

            return new SelectList(list, "value", "text", selected);
            
        }
        
        public SelectList selectList(string selected, bool flagCache = true) {

            var list = new[] {
                new{value = "R", text = "Receitas"},
                new{value = "D", text = "Despesas"},
            };

            return new SelectList(list, "value", "text", selected);
        }
        
    }
    
}