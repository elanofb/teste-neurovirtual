using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.Helpers {

    public class GaleriaFotoHelper {

        //Constanctes
	    private static GaleriaFotoHelper _instance;
		private IGaleriaFotoBL _GaleriaFotoBL;

        //Atributos

        //Propriedades
	    public static GaleriaFotoHelper getInstance => _instance = _instance ?? new GaleriaFotoHelper();
        private IGaleriaFotoBL OGaleriaFotoBL => _GaleriaFotoBL = _GaleriaFotoBL ?? new GaleriaFotoBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OGaleriaFotoBL.listar("", "S")
                                        .ToList()
                                        .Select(x => new { id = x.id, descricao = x.titulo})
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
    }
}