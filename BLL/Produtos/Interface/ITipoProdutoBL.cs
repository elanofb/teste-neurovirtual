using System;
using System.Json;
using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface ITipoProdutoBL{
        
	    IQueryable<TipoProduto> query(int? idOrganizacaoParam = null);

		TipoProduto carregar(int id);

		IQueryable<TipoProduto> listar(string valorBusca, bool? ativo);

		bool existe(string descricao, int id);

		bool salvar(TipoProduto OTipoProduto);
        
	    JsonMessageStatus alterarStatus(int id);

	    UtilRetorno excluir(int id);

	}
}