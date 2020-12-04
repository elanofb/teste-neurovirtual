using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using UTIL.Resources;

namespace BLL.Notificacoes {

    public class NotificacaoPostbackCadastroBL : DefaultBL, INotificacaoPostbackCadastroBL {
	    
        //
        public NotificacaoPostbackCadastroBL() {
            
        }

        //
        public bool salvar(NotificacaoPostback ONotificacaoPostback) {
	        
            if(ONotificacaoPostback.id > 0) {
	            return this.atualizar(ONotificacaoPostback);
            }
	        
			return this.inserir(ONotificacaoPostback);
        }

        //
        private bool inserir(NotificacaoPostback ONotificacaoPostback) {

            ONotificacaoPostback.setDefaultInsertValues();
	        
            db.NotificacaoPostback.Add(ONotificacaoPostback);
            
            db.SaveChanges();

            return ONotificacaoPostback.id > 0;
        }

        //
        private bool atualizar(NotificacaoPostback ONotificacaoPostback) {
            
            //Localizar existentes no banco
            var dbNotificacaoPostback = db.NotificacaoPostback.condicoesSeguranca().FirstOrDefault(x => x.id == ONotificacaoPostback.id);

            if (dbNotificacaoPostback == null) {
                return false;
            }

            //Atualizacao da Empresa
            var NotificacaoPostbackEntry = db.Entry(dbNotificacaoPostback);
	        
	        ONotificacaoPostback.setDefaultUpdateValues( );
	        
            NotificacaoPostbackEntry.CurrentValues.SetValues(ONotificacaoPostback);
	        
	        NotificacaoPostbackEntry.State = EntityState.Modified;
	        
            NotificacaoPostbackEntry.ignoreFields();
	        
            db.SaveChanges();
	        
            return ONotificacaoPostback.id > 0;
        }
	    
        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        
	        var retorno = new JsonMessageStatus();

            var ONotificacaoPostback = db.NotificacaoPostback.condicoesSeguranca().FirstOrDefault(x => x.id == id);

	        if (ONotificacaoPostback == null) {
	            
		        retorno.error = true;
	            
		        retorno.message = NotificationMessages.invalid_register_id;
		        
	        } else {
		        
		        ONotificacaoPostback.ativo = (ONotificacaoPostback.ativo != true);
		        
		        db.SaveChanges();
		        
		        retorno.active = ONotificacaoPostback.ativo == true ? "S" : "N";
		        
		        retorno.message = NotificationMessages.updateSuccess;    
	        }
	        
	        return retorno;
        }

    }
    
}