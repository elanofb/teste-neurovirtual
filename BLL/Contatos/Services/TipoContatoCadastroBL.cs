using DAL.Contatos;
using System;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;

namespace BLL.Contatos {

	public class TipoContatoCadastroBL : DefaultBL, ITipoContatoCadastroBL {

		//
		public TipoContatoCadastroBL() {

		}

		//
		public bool salvar(TipoContato OTipoContato) {

			if (OTipoContato.id == 0) {
				return this.inserir(OTipoContato);
			}
			
			return this.atualizar(OTipoContato);
		}
		
		//
		private int proximoId() {

			var proximoId = db.TipoContato.OrderByDescending(x => x.id).Select(x => x.id).FirstOrDefault();

			if (proximoId < 101) {
				
				proximoId = 101;
				
				return proximoId;
			}

			proximoId = proximoId + 1;
			
			return proximoId;

		}
		
		// 
		private bool inserir(TipoContato OTipoContato) {

			OTipoContato.id = this.proximoId();
			
			OTipoContato.setDefaultInsertValues();

			db.TipoContato.Add(OTipoContato);

			db.SaveChanges();
			
			return OTipoContato.id > 0;

		}
		
		// 
		private bool atualizar(TipoContato OTipoContato) {

			var dbTipoContato = db.TipoContato.condicoesSeguranca().FirstOrDefault(x => x.id == OTipoContato.id);

			if (dbTipoContato == null) {
				return false;
			}

			OTipoContato.setDefaultUpdateValues();

			var dbEntry = db.Entry(dbTipoContato);
			
			dbEntry.CurrentValues.SetValues(OTipoContato);
			
			dbEntry.ignoreFields();

			db.SaveChanges();

			return OTipoContato.id > 0;

		}
		
		//
		public JsonMessageStatus alterarStatus(int id) {
			
			var ORetorno = new JsonMessageStatus();

			var dbTipoContato = db.TipoContato.condicoesSeguranca().FirstOrDefault(x => x.id == id);

			if (dbTipoContato == null) {
				
				ORetorno.error = true;
				
				ORetorno.message = NotificationMessages.invalid_register_id;

				return ORetorno;

			} 
			
			dbTipoContato.ativo = dbTipoContato.ativo != true;
			
			db.SaveChanges();
			
			ORetorno.active = dbTipoContato.ativo == true ? "S" : "N";
			
			ORetorno.message = NotificationMessages.updateSuccess;
			
			return ORetorno;
		}

	}
}