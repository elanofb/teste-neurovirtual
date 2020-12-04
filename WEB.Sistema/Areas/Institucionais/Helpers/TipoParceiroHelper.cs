using System.Linq;
using System.Web.Mvc;
using BLL.Institucionais;

namespace WEB.Areas.Institucionais.Helpers {

    public class TipoConvenioHelper {

        //Constanctes
	    private static TipoConvenioHelper _instance;
		private ITipoConvenioBL _TipoConvenioBL;

        //Atributos

        //Propriedades
	    public static TipoConvenioHelper getInstance => _instance = _instance ?? new TipoConvenioHelper();
        private ITipoConvenioBL OTipoConvenioBL => _TipoConvenioBL = _TipoConvenioBL ?? new TipoConvenioBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OTipoConvenioBL.listar("", true)
                                        .ToList()
                                        .Select(x => new { x.id, x.descricao})
		                                .ToList();
            

            return new SelectList(lista, "id", "descricao", selected);
		}
    }
}