using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.ConfiguracoesEtiquetas;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.ConfiguracoesEtiquetas.Helpers {

    public class ConfiguracaoEtiquetaHelper {

        //Constanctes
	    private static ConfiguracaoEtiquetaHelper _instance;
        
        //Propriedades
	    public static ConfiguracaoEtiquetaHelper getInstance => _instance = _instance ?? new ConfiguracaoEtiquetaHelper();

		//
		public SelectList selectList(int? selected) {

            var lista = ConfiguracaoEtiquetaBL.getInstance.listarFromCache(HttpContext.Current.User.idOrganizacao())
                                        .Select(x => new { x.id, x.descricao}).ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
    }
}