using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using UTIL.Resources;

namespace BLL.Notificacoes {

    public class TemplateMensagemCadastroBL : DefaultBL, ITemplateMensagemCadastroBL {
	    
        //
        public TemplateMensagemCadastroBL() {
            
        }

        //
        public bool salvar(TemplateMensagem OTemplateMensagem) {

	        OTemplateMensagem.titulo = OTemplateMensagem.titulo.abreviar(100);
	        
            if(OTemplateMensagem.id > 0) {
	            return this.atualizar(OTemplateMensagem);
            }
	        
			return this.inserir(OTemplateMensagem);
        }

        //
        private bool inserir(TemplateMensagem OTemplateMensagem) {

            OTemplateMensagem.setDefaultInsertValues();
	        
            db.TemplateMensagem.Add(OTemplateMensagem);
            
            db.SaveChanges();

            return OTemplateMensagem.id > 0;
        }

        //
        private bool atualizar(TemplateMensagem OTemplateMensagem) {
            
            //Localizar existentes no banco
            var dbTemplateMensagem = db.TemplateMensagem.condicoesSeguranca().FirstOrDefault(x => x.id == OTemplateMensagem.id);

            if (dbTemplateMensagem == null) {
                return false;
            }

            //Atualizacao da Empresa
            var TemplateMensagemEntry = db.Entry(dbTemplateMensagem);
	        
	        OTemplateMensagem.setDefaultUpdateValues( );
	        
            TemplateMensagemEntry.CurrentValues.SetValues(OTemplateMensagem);
	        
	        TemplateMensagemEntry.State = EntityState.Modified;
	        
            TemplateMensagemEntry.ignoreFields();
	        
            db.SaveChanges();
	        
            return OTemplateMensagem.id > 0;
        }
	    
        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        
	        var retorno = new JsonMessageStatus();

            var OTemplateMensagem = db.TemplateMensagem.condicoesSeguranca().FirstOrDefault(x => x.id == id);

	        if (OTemplateMensagem == null) {
	            
		        retorno.error = true;
	            
		        retorno.message = NotificationMessages.invalid_register_id;
		        
	        } else {
		        
		        OTemplateMensagem.ativo = (OTemplateMensagem.ativo != true);
		        
		        db.SaveChanges();
		        
		        retorno.active = OTemplateMensagem.ativo == true ? "S" : "N";
		        
		        retorno.message = NotificationMessages.updateSuccess;    
	        }
	        
	        return retorno;
        }

    }
    
}