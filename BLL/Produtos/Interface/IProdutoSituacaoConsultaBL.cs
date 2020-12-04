using System;
using System.Json;
using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoSituacaoConsultaBL
	{
        
	    IQueryable<ProdutoSituacao> query();

		ProdutoSituacao carregar(int id);

		IQueryable<ProdutoSituacao> listar(string valorBusca, bool? ativo);

	}
}