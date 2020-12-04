using System.Web.Mvc;
using System.Linq;
using BLL.Associados;

namespace WEB.Areas.Associados.Helpers{

    public class TipoTituloHelper{

        private static TipoTituloBL _TipoTituloBL;

        public static TipoTituloBL getService(){
            if(_TipoTituloBL == null){
                _TipoTituloBL = new TipoTituloBL();
            }
            return _TipoTituloBL;
        }


		//Select lista para selecao de items
        public static SelectList selectList(int? selected, int[] excludeItens , int idInstituicao){
            var lista = getService().listar(idInstituicao, "", "S");
			if (excludeItens != null && excludeItens.Length > 0) { 
				lista = lista.Where(x => !excludeItens.Contains(x.id));
			}
            return new SelectList(lista.ToList(), "id", "descricao", selected);
        }

		//Select lista para selecao de items
        public static SelectList selectList(int idInstituicao, int? selected){
            var lista = getService().listar(idInstituicao, "", "S");
            return new SelectList(lista.ToList(), "id", "descricao", selected);
        }

    }
}