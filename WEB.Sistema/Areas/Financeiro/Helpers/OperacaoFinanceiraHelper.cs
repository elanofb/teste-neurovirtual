using System.Web.Mvc;

namespace WEB.Areas.Financeiro.Helpers {
    public class OperacaoFinanceiraHelper{
        //
        public static SelectList selectList(string selected){
            var list = new[] { 
                    new{value = "D", text = "Débito"},
                    new{value = "C", text = "Crédito"}
            };
            
            return new SelectList(list, "value", "text", selected);
        }

    }
}