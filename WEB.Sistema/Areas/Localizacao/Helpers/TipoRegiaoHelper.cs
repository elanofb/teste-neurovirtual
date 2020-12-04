using System.Linq;
using System.Web.Mvc;
using BLL.Localizacao;


namespace WEB.Areas.Localizacao.Helpers {

	public class TipoRegiaoHelper {
		private static TipoRegiaoBL _TipoRegiaoBL;

		public static TipoRegiaoBL getService() {
			if (_TipoRegiaoBL == null) {
				_TipoRegiaoBL = new TipoRegiaoBL();
			}
			return _TipoRegiaoBL;
		}

		//
		public static SelectList getComboList(int? selected) {

			var list = getService().listar("", "S").OrderBy(x => x.descricao).ToList();

			return new SelectList(list, "id", "descricao", selected);
		}
	}
}