using System;
using System.Linq;
using BLL.Services;
using DAL.Produtos;

namespace BLL.Produtos {

	public class ProdutoComposicaoConsultaBL : DefaultBL, IProdutoComposicaoConsultaBL {

		//
		public ProdutoComposicaoConsultaBL() {
		}

	    //
	    public IQueryable<ProdutoComposicao> query() {

	        var query = from Obj in db.ProdutoComposicao
	                    where Obj.flagExcluido == false
	                    select Obj;
            
	        return query;

	    }

		//Carregamento de registro pelo ID
		public ProdutoComposicao carregar(int id) { 

		    var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);

		}

		//Listagem de registros de acordo com filtros
		public IQueryable<ProdutoComposicao> listar(bool? ativo) {

			var query = this.query().condicoesSeguranca();
            
			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			return query;
		}

	}
}