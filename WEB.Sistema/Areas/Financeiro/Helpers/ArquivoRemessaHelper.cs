using System.Web.Mvc;

namespace WEB.Areas.Financeiro.Helpers{
    public class ArquivoRemessaHelper{

        public static SelectList selectList(string selected){
            var list = new[] { 
                new{value = "G", text = "Gerado"},
                new{value = "NG", text = "Não Gerado"}
            };
            
            return new SelectList(list, "value", "text", selected);
        }
    }
}