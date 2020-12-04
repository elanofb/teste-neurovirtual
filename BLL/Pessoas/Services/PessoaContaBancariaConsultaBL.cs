using System;
using System.Linq; 
using BLL.Services;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public class PessoaContaBancariaConsultaBL : DefaultBL, IPessoaContaBancariaConsultaBL {

		//
		public PessoaContaBancariaConsultaBL() { }

		// 
		public IQueryable<PessoaContaBancaria> query(int? idOrganizacaoParam = null) {
            
			var query = from PA in db.PessoaContaBancaria 
				where PA.dtExclusao == null 
				select PA;

			if (idOrganizacaoParam.toInt() == 0) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;
		}

		//Carregamento de registro pelo ID
		public PessoaContaBancaria carregar(int id) {
            
			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);
            
		}
		
		//Listagem de registros de acordo com filtros
		public IQueryable<PessoaContaBancaria> listar(int idPessoa) {
            
			var query = this.query().condicoesSeguranca();
			
			if (idPessoa > 0) {
				query = query.Where(x => x.idPessoa == idPessoa);
			}

			return query;
		}

	}
    
}