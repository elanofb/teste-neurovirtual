using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.Helpers {

    public class CategoriaNoticiaHelper {

        //Constanctes
	    private static CategoriaNoticiaHelper _instance;
		private ICategoriaNoticiaBL _CategoriaNoticiaBL;

        //Atributos

        //Propriedades
	    public static CategoriaNoticiaHelper getInstance => _instance = _instance ?? new CategoriaNoticiaHelper();
        private ICategoriaNoticiaBL OCategoriaNoticiaBL => _CategoriaNoticiaBL = _CategoriaNoticiaBL ?? new CategoriaNoticiaBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OCategoriaNoticiaBL.listar("", true)
                                        .ToList()
                                        .Select(x => new { id = x.id, descricao = x.descricao})
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
    }
}