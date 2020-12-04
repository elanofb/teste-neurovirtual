using System.Web.Mvc;
using System.Linq;
using BLL.Fornecedores;

namespace WEB.Areas.Fornecedores.Helpers {

	public class FornecedorHelper {

        //
        private static FornecedorHelper _instance;
		private static IFornecedorConsultaBL _FornecedorConsultaBL;
        
        //
	    public static FornecedorHelper getInstance => _instance = _instance ?? new FornecedorHelper();
		public static IFornecedorConsultaBL OFornecedorConsultaBL => _FornecedorConsultaBL = _FornecedorConsultaBL ?? new FornecedorConsultaBL();

		//
		public SelectList selectList(int? selected) {

			var lista = OFornecedorConsultaBL.listar("", true)
									 .Select(x => new { x.id, x.Pessoa.nome }).ToList();

			return new SelectList(lista.ToList(), "id", "nome", selected);

		}

	}

}