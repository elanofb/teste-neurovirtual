using System;
using System.Linq;
using BLL.Services;
using DAL.Frete;

namespace BLL.Frete {
	public class TipoFreteBL : DefaultBL, ITipoFreteBL {

		//Atributos

		//Propriedades

		//Construtor
		public TipoFreteBL() {

		}

		//Carregar registro a partir do ID
		public TipoFrete carregar(int id) { 

			var query = from Tip in db.TipoFrete
						where
							Tip.id == id && 
							Tip.flagExcluido == false
						select	
							Tip;

			var Retorno = query.FirstOrDefault();

			return Retorno;
		}

		//Listagem 
		public IQueryable<TipoFrete> listar(string valorBusca, bool? ativo){

			var query = from Tip in db.TipoFrete
						where	
							Tip.flagExcluido == false
						select
							Tip;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.descricaoCliente.Contains(valorBusca));
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}
 			
			return query;
		}
	}
}
