using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.Helpers {

    public class TipoGaleriaFotoHelper {

        //Constanctes
	    private static TipoGaleriaFotoHelper _instance;
		private ITipoGaleriaFotoBL _TipoGaleriaFotoBL;

        //Atributos

        //Propriedades
	    public static TipoGaleriaFotoHelper getInstance => _instance = _instance ?? new TipoGaleriaFotoHelper();
        private ITipoGaleriaFotoBL OTipoGaleriaFotoBL => _TipoGaleriaFotoBL = _TipoGaleriaFotoBL ?? new TipoGaleriaFotoBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OTipoGaleriaFotoBL.listar("", true)
                                        .ToList()
                                        .Select(x => new { id = x.id, descricao = x.descricao})
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
    }
}