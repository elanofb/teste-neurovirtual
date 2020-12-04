using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesTextos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesTextos {

	public class IdiomaExclusaoBL : DefaultBL, IIdiomaExclusaoBL {

		//
		public IdiomaExclusaoBL() {

		}

		//
		public JsonMessage excluir(int id) {
			
			var ORetorno = new JsonMessage();
			ORetorno.error = false;
			
			db.Idioma.condicoesSeguranca().Where(x => x.id == id)
			  .Update(x => new Idioma { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });

			ORetorno.error = db.Idioma.Any(x => x.id == id && !x.dtExclusao.HasValue);

			if (ORetorno.error) {
				
				ORetorno.message = "Houve algum problema ao excluir o idioma informado.";
				
				return ORetorno;
			}
			
			return ORetorno;

		}

	}
}