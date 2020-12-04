using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;

namespace WEB.Areas.Produtos.Helpers {

    public class ProdutoSituacaoHelper {

        //Constanctes
	    private static ProdutoSituacaoHelper _instance;
		private IProdutoSituacaoConsultaBL _ProdutoSituacaoConsultaBL;

        //Atributos

        //Propriedades
	    public static ProdutoSituacaoHelper getInstance => _instance = _instance ?? new ProdutoSituacaoHelper();
        private IProdutoSituacaoConsultaBL OProdutoSituacaoConsultaBL => _ProdutoSituacaoConsultaBL = _ProdutoSituacaoConsultaBL ?? new ProdutoSituacaoConsultaBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OProdutoSituacaoConsultaBL.listar("", true)
                                        .Select(x => new { x.id, x.descricao })
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
        
    }
}