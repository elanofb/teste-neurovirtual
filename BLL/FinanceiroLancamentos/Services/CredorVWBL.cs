using System;
using System.Linq;
using DAL.FinanceiroLancamentos;
using BLL.Services;

namespace BLL.FinanceiroLancamentos {

	public class CredorVWBL : DefaultBL, ICredorVWBL {

	    public const string keyCache = "credor";

		//
		public CredorVWBL(){
		}

        //Carregamento de registro pelo ID
        public CredorVW carregar(int id) {

            var query = from P in db.CredorVW
                        where P.idPessoa == id
						select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();

        }

        //Listagem de registros de acordo com filtros
		public IQueryable<CredorVW> listar(string valorBusca) {

			var query = from P in db.CredorVW
						select P;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.nome.Contains(valorBusca) || x.razaoSocial.Contains(valorBusca) || x.nroDocumento.Contains(valorBusca));
			}

			return query;
		}
    }
}