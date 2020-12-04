using DAL.Contatos;
using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Relacionamentos;
using UTIL.Resources;

namespace BLL.Relacionamentos {

	public class OcorrenciaRelacionamentoCadastroBL : DefaultBL, IOcorrenciaRelacionamentoCadastroBL {

		//
		public OcorrenciaRelacionamentoCadastroBL() {

		}

		//
		public bool salvar(OcorrenciaRelacionamento OOcorrenciaRelacionamento) {

			if (OOcorrenciaRelacionamento.id == 0) {
				return this.inserir(OOcorrenciaRelacionamento);
			}
			
			return this.atualizar(OOcorrenciaRelacionamento);
		}
		
		//
		private int proximoId() {

			var proximoId = db.OcorrenciaRelacionamento.OrderByDescending(x => x.id).Select(x => x.id).FirstOrDefault();

			if (proximoId < 101) {
				
				proximoId = 101;
				
				return proximoId;
			}

			proximoId = proximoId + 1;
			
			return proximoId;

		}
		
		// 
		private bool inserir(OcorrenciaRelacionamento OOcorrenciaRelacionamento) {

			OOcorrenciaRelacionamento.id = this.proximoId();
			
			OOcorrenciaRelacionamento.setDefaultInsertValues();

			db.OcorrenciaRelacionamento.Add(OOcorrenciaRelacionamento);

			db.SaveChanges();
			
			return OOcorrenciaRelacionamento.id > 0;

		}
		
		// 
		private bool atualizar(OcorrenciaRelacionamento OOcorrenciaRelacionamento) {

			var dbOcorrenciaRelacionamento = db.OcorrenciaRelacionamento.condicoesSeguranca().FirstOrDefault(x => x.id == OOcorrenciaRelacionamento.id);

			if (dbOcorrenciaRelacionamento == null) {
				return false;
			}

			OOcorrenciaRelacionamento.setDefaultUpdateValues();

			var dbEntry = db.Entry(dbOcorrenciaRelacionamento);
			
			dbEntry.CurrentValues.SetValues(OOcorrenciaRelacionamento);
			
			dbEntry.ignoreFields();

			db.SaveChanges();

			return OOcorrenciaRelacionamento.id > 0;

		}
		
		//
		public JsonMessageStatus alterarStatus(int id) {
			
			var ORetorno = new JsonMessageStatus();

			var dbOcorrenciaRelacionamento = db.OcorrenciaRelacionamento.condicoesSeguranca().FirstOrDefault(x => x.id == id);

			if (dbOcorrenciaRelacionamento == null) {
				
				ORetorno.error = true;
				
				ORetorno.message = NotificationMessages.invalid_register_id;

				return ORetorno;

			} 
			
			dbOcorrenciaRelacionamento.ativo = dbOcorrenciaRelacionamento.ativo != true;
			
			db.SaveChanges();
			
			ORetorno.active = dbOcorrenciaRelacionamento.ativo == true ? "S" : "N";
			
			ORetorno.message = NotificationMessages.updateSuccess;
			
			return ORetorno;
		}

	}
}