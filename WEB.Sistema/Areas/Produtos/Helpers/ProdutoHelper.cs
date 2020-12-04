using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using DAL.Produtos;

namespace WEB.Areas.Produtos.Helpers {

    public class ProdutoHelper {

        //Constanctes
	    private static ProdutoHelper _instance;
		private IProdutoBL _ProdutoBL;

        //Atributos

        //Propriedades
	    public static ProdutoHelper getInstance => _instance = _instance ?? new ProdutoHelper();
        private IProdutoBL OProdutoBL => _ProdutoBL = _ProdutoBL ?? new ProdutoBL();

		//
		public SelectList selectList(int idTipoProduto, int? selected) {

            var lista = this.OProdutoBL.listar(idTipoProduto, "", true)
                                        .ToList()
                                        .Select(x => new { id = x.id, descricao = x.nome})
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}

        public static SelectList selectProdServ(string selected) {

            var lista = new[] {
                new{value = ProdutoConst.PRODUTO, text = "Produto"},
                new{value = ProdutoConst.SERVICO, text = "Serviço"},
            };

            return new SelectList(lista, "value", "text", selected);
        }
    }
}