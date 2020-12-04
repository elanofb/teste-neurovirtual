using System;
using System.Json;
using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IProdutoComposicaoConsultaBL{
        
	    IQueryable<ProdutoComposicao> query();

		ProdutoComposicao carregar(int id);

		IQueryable<ProdutoComposicao> listar(bool? ativo);

	}
}