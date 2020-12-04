using System;
using System.Json;
using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoItemConsultaBL{
        
	    IQueryable<ProdutoItem> query();

		ProdutoItem carregar(int id);

		IQueryable<ProdutoItem> listar(string valorBusca, bool? ativo);

	}
}