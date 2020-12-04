using System.Web.Mvc;
using BLL.Financeiro;

namespace WEB.Areas.Planos.Helpers{

    public class FormaPagamentoHelper{

        private static FormaPagamentoBL _FormaPagamentoBL;

        public static FormaPagamentoBL getService(){
            if(_FormaPagamentoBL == null){
                _FormaPagamentoBL = new FormaPagamentoBL();
            }
            return _FormaPagamentoBL;
        }

        //
        public static SelectList selectList(int? selected){
            var list = getService().listar("", "S");
            return new SelectList(list, "id", "descricao", selected);
        }

    }
}