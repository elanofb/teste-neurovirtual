using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {
	public class ConfiguracaoMembroConsultaBL : DefaultBL, IConfiguracaoMembroConsultaBL {
		
		/// <summary>
		/// 
		/// </summary>
		public IQueryable<ConfiguracaoMembro> query(int? idOrganizacaoParam = null) {

			var query = from CM in db.ConfiguracaoMembro
						where CM.dtExclusao == null
						select CM;
            
			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;

		}
		
		//Carregamento de registro pelo ID
		public ConfiguracaoMembro carregar(int id) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);
		}
		
		//Carregamento de registro pelo ID
		public ConfiguracaoMembro carregarPorMembro(int idMembro) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.idMembro == idMembro);
		}
		
		//listar registros do banco com base nos parametros
		public IQueryable<ConfiguracaoMembro> listar(bool? ativo) {

			var query = this.query();

			query = query.condicoesSeguranca();

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
	}
}