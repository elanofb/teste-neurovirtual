using System.Web.Mvc;
using System.Linq;
using BLL.Planos;

namespace WEB.Areas.Planos.Helpers{

    public class PlanoHelper{

        private static PlanoBL _PlanoBL;

        public static PlanoBL getService(){
            _PlanoBL = _PlanoBL ?? new PlanoBL();
            return _PlanoBL;
        }

        //
        public static SelectList selectList(int selected){
            var list = getService().listar("", "S").OrderBy(x => x.nome).ToList();
            return new SelectList(list, "id", "nome", selected);
        }

        /**
		*
		*/
        public static SelectList getComboYesNo(string selected) {
            var list = new[] { 
                    new{value = "S", text = "Sim"},
                    new{value = "N", text = "Não"}
            };
            return new SelectList(list, "value", "text", selected);
        }

    }
}