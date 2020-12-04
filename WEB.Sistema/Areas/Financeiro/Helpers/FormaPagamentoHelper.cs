using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.Helpers{
    public class FormaPagamentoHelper{

        //Atributos
        private static IFormaPagamentoBL _FormaPagamentoBL;
        //Propriedades
        private static IFormaPagamentoBL OFormaPagamentoBL {get{ return (_FormaPagamentoBL = _FormaPagamentoBL ?? new FormaPagamentoBL());}}


        public static SelectList selectList(int? selected) {
            var list = OFormaPagamentoBL.listar("","S").OrderBy(x => x.descricao);
            return new SelectList(list, "id", "descricao", selected);
        }
    }
}