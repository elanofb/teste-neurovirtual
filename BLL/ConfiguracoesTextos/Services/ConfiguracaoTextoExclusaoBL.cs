using System;
using System.Linq;
using BLL.Services;
using System.Json;
using DAL.ConfiguracoesTextos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesTextos {

	public class ConfiguracaoTextoExclusaoBL: DefaultBL, IConfiguracaoTextoExclusaoBL {

		public JsonMessage excluir(string key, int? idOrganizacaoParam = null) {

			var ORetorno = new JsonMessage();
			
			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}
			
			if (!(idOrganizacaoParam > 0) || String.IsNullOrEmpty(key)) {

				ORetorno.error = true;
				
				ORetorno.message = "O registro informado não foi encontrado.";

				return ORetorno;

			}

			db.ConfiguracaoTexto.Where(x => x.idOrganizacao == idOrganizacaoParam && x.key == key)
				.Update(x => new ConfiguracaoTexto { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });

			ORetorno.error = false;

			ORetorno.message = "O registro foi removido com sucesso.";

			return ORetorno;

		}

	}
}