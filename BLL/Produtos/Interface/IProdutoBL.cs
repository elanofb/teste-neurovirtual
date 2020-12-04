using System;
using System.Json;
using System.Linq;
using DAL.Produtos;
using System.Web;

namespace BLL.Produtos {

	public interface IProdutoBL{

	    IQueryable<Produto> query(int? idOrganizacaoParam = null);

		Produto carregar(int idProduto);

		IQueryable<Produto> listar(int idTipoProduto, string valorBusca, bool? ativo, string flagProdServ = "");

		IQueryable<ProdutoAutoComplete> autocompletar(string valorBusca);

	    bool salvar(Produto OProduto, HttpPostedFileBase OImagem);
        
		bool existe(string nome, int idDesconsiderado);

	    JsonMessageStatus alterarStatus(int id);

	    UtilRetorno excluir(int id);

        void atualizarEstoque(int idProduto, int quantidadeOperacao, string tipoAcao = "add");

	}
}