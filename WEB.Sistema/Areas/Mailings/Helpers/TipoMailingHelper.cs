using System.Web.Mvc;
using System.Linq;
using BLL.Mailings;
using DAL.Associados;
using DAL.Mailings;

namespace WEB.Areas.Mailings.Helpers {

	public class TipoMailingHelper {

		private static TipoMailingBL _TipoMailingBL;

		public static TipoMailingBL getService() {
		    return _TipoMailingBL ?? (_TipoMailingBL = new TipoMailingBL());
		}

	    //
		public static SelectList selectList(int? selected, int idTipoAssociado = 0) {
		    var lista = getService().listar("S", "")
		        .Select(x => new {x.id, x.nome});

            if (idTipoAssociado == TipoAssociadoConst.FORNECEDOR) {
                lista = lista.Where(x => x.id == TipoMailingConst.FORNECEDORES);
            }

			return new SelectList(lista.ToList(), "id", "nome", selected);
		}
	}
}