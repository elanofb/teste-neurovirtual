using System.Linq;
using System.Web.Mvc;
using BLL.Permissao;

namespace WEB.Areas.Permissao.Helpers{
	public class RecursoHelper {

        private static AcessoRecursoBL _RecursoGrupoBL;

        public static AcessoRecursoBL getService(){
            if(_RecursoGrupoBL == null){
                _RecursoGrupoBL = new AcessoRecursoBL();
            }
            return _RecursoGrupoBL;
        }

        //
        public static SelectList selectList(int? selected, int idRecursoGrupo){

            var list = getService().listar(idRecursoGrupo, 0, "S").ToList().OrderBy(x => x.descricao);

            return new SelectList(list, "id", "nomeDisplay", selected);
        }
	}
}