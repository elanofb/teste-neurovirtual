using System.Web.Mvc;
using System.Linq;
using BLL.Cargos;

namespace WEB.Areas.Cargos.Helpers {
	
	public class CargoHelper {

		//Atributos
		private static CargoHelper _instance;
		private ICargoBL _CargoBL;
        
		//Propriedades
		public static CargoHelper getInstance => _instance = _instance ?? new CargoHelper();
		private ICargoBL OCargoBL => this._CargoBL = this._CargoBL ?? new CargoBL();

		
		public SelectList selectList(int? selected, int[] excludeItens) {

			var query = this.OCargoBL.listar("", "S");

			if (excludeItens != null && excludeItens.Length > 0) {
				query = query.Where(x => !excludeItens.Contains(x.id));
			}

			var lista = query.ToList();

			var subList = lista.Select(x => new { value = x.id, text = x.descricao }).OrderBy(x => x.text).ToList();

			return new SelectList(subList, "value", "text", selected);
		}			
    }
}