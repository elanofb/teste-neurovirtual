using System.Linq;
using BLL.Configuracoes;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using DAL.Pessoas;

namespace BLL.AssociadosContribuicoes {

	public class AssociadoContribuicaoCobrancaBL : DefaultBL, IAssociadoContribuicaoCobrancaBL {

		//Atributos

		//Propriedades

		//Carregar um registro para realizacao do envio
		public AssociadoContribuicaoEmailCobranca carregar(int idAssociadoContribuicao, bool? flagEnviado) { 

			var query = from Em in this.db.AssociadoContribuicaoEmailCobranca
						where 
							Em.idAssociadoContribuicao == idAssociadoContribuicao &&
							Em.flagExcluido == false 
						select
							Em;

			if (flagEnviado == true) { 
				query = query.Where(x => x.flagEnvio == flagEnviado);
			}

			if (flagEnviado == false) { 
				query = query.Where(x => x.flagEnvio == flagEnviado);
			}

			return query.FirstOrDefault();
		}

		//Listagem dos registros de acordo com os parametros
		public IQueryable<AssociadoContribuicaoEmailCobranca> listar(int idTarefa, bool? flagEnviado) {

			var query = from Cob in db.AssociadoContribuicaoEmailCobranca
						where Cob.flagExcluido == false
						select Cob;

			if (idTarefa > 0) { 
				query = query.Where(x => x.idTarefa == idTarefa);
			}

			if (flagEnviado == true) { 
				query = query.Where(x => x.flagEnvio == flagEnviado);
			}

			if (flagEnviado == false) { 
				query = query.Where(x => x.flagEnvio == flagEnviado);
			}

			return query;
		}



	}
}