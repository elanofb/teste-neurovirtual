using System.Web.Mvc;
using BLL.Planos;

namespace WEB.Areas.Planos.Helpers{

    public class PlanoPeriodoHelper{

        private static PlanoPeriodoBL _PlanoPeriodoBL;

        public static PlanoPeriodoBL getService(){
            if(_PlanoPeriodoBL == null){
                _PlanoPeriodoBL = new PlanoPeriodoBL();
            }
            return _PlanoPeriodoBL;
        }

        //
        public static SelectList selectList(int selected){
            var list = getService().listar("", "S");
            return new SelectList(list, "id", "descricao", selected);
        }

    }
}