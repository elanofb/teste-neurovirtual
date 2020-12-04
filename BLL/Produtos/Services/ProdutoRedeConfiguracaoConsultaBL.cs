using System;
using System.Linq;
using BLL.Services;
using DAL.Produtos;

namespace BLL.Produtos {

	public class ProdutoRedeConfiguracaoConsultaBL : DefaultBL, IProdutoRedeConfiguracaoConsultaBL {

		//
		public ProdutoRedeConfiguracaoConsultaBL() {
		}

	    //
	    public IQueryable<ProdutoRedeConfiguracao> query() {

	        var query = from Obj in db.ProdutoRedeConfiguracao
	                    where Obj.dtExclusao == null
	                    select Obj;
            
	        return query;

	    }

		//Carregamento de registro pelo ID
		public ProdutoRedeConfiguracao carregar(int id) { 

		    var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);

		}

		//Listagem de registros de acordo com filtros
		public IQueryable<ProdutoRedeConfiguracao> listar(int idProduto) {

			var query = this.query().condicoesSeguranca();
            
			if (idProduto > 0) {
				query = query.Where(x => x.idProduto == idProduto);
			}
            
			return query;
		}

	}
}