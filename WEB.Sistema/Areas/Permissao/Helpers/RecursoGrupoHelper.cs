using System.Linq;
using System.Web.Mvc;
using BLL.Permissao;

namespace WEB.Areas.Permissao.Helpers{
	public class RecursoGrupoHelper {

        private static AcessoRecursoGrupoBL _RecursoGrupoBL;

        public static AcessoRecursoGrupoBL getService(){
            if(_RecursoGrupoBL == null){
                _RecursoGrupoBL = new AcessoRecursoGrupoBL();
            }
            return _RecursoGrupoBL;
        }

        //
        public static SelectList selectList(int? selected){
            var list = getService().listar("S").OrderBy(x => x.descricao);

            return new SelectList(list, "id", "descricao", selected);
        }
	}
}