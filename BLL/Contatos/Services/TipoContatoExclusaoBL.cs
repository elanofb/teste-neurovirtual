using DAL.Contatos;
using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Contatos {

	public class TipoContatoExclusaoBL : DefaultBL, ITipoContatoExclusaoBL {

		//
		public TipoContatoExclusaoBL() {

		}

		//
		public JsonMessage excluir(int[] ids) {
			
			var ORetorno = new JsonMessage();
			ORetorno.error = false;
			
			db.TipoContato.condicoesSeguranca().Where(x => ids.Contains(x.id))
			  .Update(x => new TipoContato { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });

			ORetorno.error = db.TipoContato.Any(x => ids.Contains(x.id) && !x.dtExclusao.HasValue);

			if (ORetorno.error) {
				
				ORetorno.message = "Houve algum problema ao excluir o(s) tipo(s) de contato(s) informado(s).";
				
				return ORetorno;
			}
			
			return ORetorno;

		}

	}
}