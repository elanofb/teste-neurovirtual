using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Relacionamentos;
using EntityFramework.Extensions;

namespace BLL.Relacionamentos {

	public class OcorrenciaRelacionamentoExclusaoBL : DefaultBL, IOcorrenciaRelacionamentoExclusaoBL {

		//
		public OcorrenciaRelacionamentoExclusaoBL() {

		}

		//
		public JsonMessage excluir(int[] ids) {
			
			var ORetorno = new JsonMessage();
			ORetorno.error = false;
			
			db.OcorrenciaRelacionamento.condicoesSeguranca().Where(x => ids.Contains(x.id))
			  .Update(x => new OcorrenciaRelacionamento { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });

			ORetorno.error = db.TipoContato.Any(x => ids.Contains(x.id) && !x.dtExclusao.HasValue);

			if (ORetorno.error) {
				
				ORetorno.message = "Houve algum problema ao excluir a(s) informada(s).";
				
				return ORetorno;
			}
			
			return ORetorno;

		}

	}
}