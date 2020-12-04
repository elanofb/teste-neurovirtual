using System.Web.Mvc;
using System.Linq;
using BLL.Diretorias;

namespace WEB.Areas.Diretorias.Helpers{

    public class DiretoriaHelper {

        private static DiretoriaBL _DiretoriaBL;

        public static DiretoriaBL getService(){
            if(_DiretoriaBL == null){
                _DiretoriaBL = new DiretoriaBL();
            }
            return _DiretoriaBL;
        }


		//Select lista para selecao de items
        public static SelectList selectList(int? selected, int[] excludeItens){
            var lista = getService().listar("", true).Select(x => new { id = x.id, descricao = x.anoInicioGestao + " - " + x.anoFimGestao });
            if (excludeItens != null && excludeItens.Length > 0) { 
				lista = lista.Where(x => !excludeItens.Contains(x.id));
			}
            return new SelectList(lista.ToList(), "id", "descricao", selected);
        }

		//Select lista para selecao de items
        public static SelectList selectList(int? selected){
            var lista = getService().listar("", true).Select(x => new { id = x.id , descricao = x.anoInicioGestao + " - " + x.anoFimGestao});

            return new SelectList(lista.ToList(), "id", "descricao", selected);
        }

    }
}