using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesTextos;
using UTIL.Resources;

namespace BLL.ConfiguracoesTextos {

	public class IdiomaCadastroBL : DefaultBL, IIdiomaCadastroBL {

		//
		public IdiomaCadastroBL() {

		}

		//
		public bool salvar(Idioma OIdioma) {

			OIdioma.descricao = OIdioma.descricao.abreviar(100);
			
			OIdioma.sigla = OIdioma.sigla.abreviar(5).stringOrEmptyLower();
			
			if (OIdioma.id == 0) {
				return this.inserir(OIdioma);
			}
			
			return this.atualizar(OIdioma);
		}
		
		// 
		private bool inserir(Idioma OIdioma) {
			
			OIdioma.setDefaultInsertValues();

			db.Idioma.Add(OIdioma);

			db.SaveChanges();
			
			return OIdioma.id > 0;

		}
		
		// 
		private bool atualizar(Idioma OIdioma) {

			var dbIdioma = db.Idioma.condicoesSeguranca().FirstOrDefault(x => x.id == OIdioma.id);

			if (dbIdioma == null) {
				return false;
			}

			OIdioma.setDefaultUpdateValues();

			var dbEntry = db.Entry(dbIdioma);
			
			dbEntry.CurrentValues.SetValues(OIdioma);
			
			dbEntry.ignoreFields();

			db.SaveChanges();

			return OIdioma.id > 0;

		}
		
		//
		public JsonMessageStatus alterarStatus(int id) {
			
			var ORetorno = new JsonMessageStatus();

			var dbIdioma = db.Idioma.condicoesSeguranca().FirstOrDefault(x => x.id == id);

			if (dbIdioma == null) {
				
				ORetorno.error = true;
				
				ORetorno.message = NotificationMessages.invalid_register_id;

				return ORetorno;

			} 
			
			dbIdioma.ativo = dbIdioma.ativo != true;
			
			db.SaveChanges();
			
			ORetorno.active = dbIdioma.ativo == true ? "S" : "N";
			
			ORetorno.message = NotificationMessages.updateSuccess;
			
			return ORetorno;
		}

	}
}