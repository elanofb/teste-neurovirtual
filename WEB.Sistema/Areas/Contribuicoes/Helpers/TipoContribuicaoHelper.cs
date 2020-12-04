using System.Web.Mvc;
using System.Linq;
using BLL.Contribuicoes;

namespace WEB.Helpers {
	public class TipoContribuicaoHelper {

		private static TipoContribuicaoBL _TipoContribuicaoBL;

		public static TipoContribuicaoBL getService() {
			if (_TipoContribuicaoBL == null) {
				_TipoContribuicaoBL = new TipoContribuicaoBL();
			}
			return _TipoContribuicaoBL;
		}

		/**
		 *
		 */
		public static SelectList selectList(int? selected) {
			var lista = getService().listar("S");

			return new SelectList(lista.ToList(), "id", "descricao", selected);
		}

	}
}