using System;
using System.Linq;
using BLL.Services;
using DAL.Produtos;

namespace BLL.Produtos {

	public class ProdutoSituacaoConsultaBL : DefaultBL, IProdutoSituacaoConsultaBL {

		//
		public ProdutoSituacaoConsultaBL() {
		}

	    //
	    public IQueryable<ProdutoSituacao> query() {

	        var query = from Obj in db.ProdutoSituacao
							where Obj.flagExcluido == false
						select Obj;
            
	        return query;

	    }

		//Carregamento de registro pelo ID
		public ProdutoSituacao carregar(int id) { 

		    var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);
		}

		//Listagem de registros de acordo com filtros
		public IQueryable<ProdutoSituacao> listar(string valorBusca, bool? ativo) {

			var query = this.query();

			if (!string.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

	}
}