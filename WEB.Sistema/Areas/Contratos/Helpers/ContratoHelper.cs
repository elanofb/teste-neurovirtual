using System.Web.Mvc;
using BLL.Contratos;

namespace WEB.Areas.Contratos.Helpers {
	public class ContratoHelper {

		//Atributos
		private static IContratoBL _ContratoBL;

		//Propriedades
		private static IContratoBL OContratoBL { get { return (_ContratoBL = _ContratoBL ?? new ContratoBL()); } }

		//
		public static SelectList getComboList(int? selected) {
			var list = OContratoBL.listar("", "S");
			return new SelectList(list, "id", "descricao", selected);
		}

	}
}