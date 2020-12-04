using System;
using System.Linq;
using BLL.Caches;
using DAL.Associados;
using BLL.Services;
using EntityFramework.Extensions;

namespace BLL.Associados {
	
	public class ConfiguracaoMembroCadastroBL : DefaultBL, IConfiguracaoMembroCadastroBL {

		private readonly string chaveCache = "configuracao_membro";
		
		/// <summary>
		/// Salvar configuracoes de Scripts e remover os registros anteriores.
		/// </summary>
		public bool salvar(ConfiguracaoMembro OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoMembro.Add(OConfiguracoes);

			db.SaveChanges();

			bool flagSucesso = OConfiguracoes.id > 0;

			int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

			if (flagSucesso) {

				db.ConfiguracaoMembro
					.Where(x => x.dtExclusao == null && x.idMembro == OConfiguracoes.idMembro && x.id != OConfiguracoes.id)
					.Update(x => new ConfiguracaoMembro { dtExclusao = DateTime.Now });

				CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
			}

			return (OConfiguracoes.id > 0);

		}

	}

}