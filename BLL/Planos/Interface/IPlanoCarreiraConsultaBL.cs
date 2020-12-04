using System;
using System.Json;
using System.Linq;
using DAL.Planos;
using DAL.Produtos;

namespace BLL.Planos {

	public interface IPlanoCarreiraConsultaBL{
        
	    IQueryable<PlanoCarreira> query(int? idOrganizacaoParam = null);
		
		PlanoCarreira carregar(int id);
		
		IQueryable<PlanoCarreira> listar(string valorBusca, bool? ativo);
		
		bool existe(string descricao, int id);

	}
}