using System;
using System.Linq;
using BLL.Services;
using DAL.Planos;
using DAL.Produtos;

namespace BLL.Planos {

	public class PlanoCarreiraConsultaBL : DefaultBL, IPlanoCarreiraConsultaBL {
		
		//
		public PlanoCarreiraConsultaBL() {
			
		}
		
	    //
	    public IQueryable<PlanoCarreira> query(int? idOrganizacaoParam = null) {
			
	        var query = from Obj in db.PlanoCarreira
	                    where Obj.dtExclusao == null
	                    select Obj;
		    
		    if (idOrganizacaoParam == null) {
			    idOrganizacaoParam = idOrganizacao;
		    }

		    if (idOrganizacaoParam > 0) {
			    query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }
            
	        return query;

	    }

		//Carregamento de registro pelo ID
		public PlanoCarreira carregar(int id) { 

		    var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);

		}
		
		//Listagem de registros de acordo com filtros
		public IQueryable<PlanoCarreira> listar(string valorBusca, bool? ativo) {
			
			var query = this.query().condicoesSeguranca();
			
			if (!valorBusca.isEmpty()){
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}
            
			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			return query;
		}
		
		//
		public bool existe(string descricao, int id) {

			var query = from C in db.PlanoCarreira
				where C.descricao == descricao && C.id != id && C.dtExclusao == null
				select C;
			
			query = query.condicoesSeguranca();
			
			var OPlano = query.Take(1).FirstOrDefault();
			
			return (OPlano == null ? false : true);

		}
		

	}
}