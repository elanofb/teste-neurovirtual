using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;

namespace WEB.Areas.Produtos.Helpers {

    public class ProdutoItemHelper {

        //Constanctes
	    private static ProdutoItemHelper _instance;
		private IProdutoItemConsultaBL _ProdutoItemConsultaBL;

        //Atributos

        //Propriedades
	    public static ProdutoItemHelper getInstance => _instance = _instance ?? new ProdutoItemHelper();
        private IProdutoItemConsultaBL OProdutoItemConsultaBL => _ProdutoItemConsultaBL = _ProdutoItemConsultaBL ?? new ProdutoItemConsultaBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OProdutoItemConsultaBL.listar("", true)
                                        .Select(x => new { x.id, x.descricao })
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
        
    }
}