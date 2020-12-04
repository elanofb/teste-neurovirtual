using System.Web.Mvc;

namespace WEB.Areas.Contratos.Helpers {
    public class SituacaoContratoHelper{
        //
        public static SelectList getSituacaoContrato(string selected){
            var list = new[] { 
                    new{value = "VIG", text = "Vigênte"},
                    new{value = "VEN", text = "Vencido"},
                    new{value = "CAN", text = "Cancelado"},
                    new{value = "ACO", text = "Acordado"}
            };
            
            return new SelectList(list, "value", "text", selected);
        }

    }
}