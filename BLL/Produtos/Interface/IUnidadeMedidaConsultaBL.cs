using System;
using System.Json;
using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos {

	public interface IUnidadeMedidaConsultaBL{
        
	    IQueryable<UnidadeMedida> query();

		UnidadeMedida carregar(int id);

		IQueryable<UnidadeMedida> listar(string valorBusca, bool? ativo);

	}
}