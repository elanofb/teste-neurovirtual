using System;
using System.Linq;
using DAL.FinanceiroLancamentos;
using BLL.Services;

namespace BLL.FinanceiroLancamentos {

	public class DevedorVWBL : DefaultBL, IDevedorVWBL {

		//
		public const string keyCache = "devedor";
		
		//
		public DevedorVWBL(){
		}

        //Carregamento de registro pelo ID
        public DevedorVW carregar(int id) {

            var query = from P in db.DevedorVW
                        where P.idPessoa == id
						select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();

        }

        //Listagem de registros de acordo com filtros
		public IQueryable<DevedorVW> listar(string valorBusca) {

			var query = from P in db.DevedorVW
						select P;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.nome.Contains(valorBusca));
			}

			return query;
		}
    }
}