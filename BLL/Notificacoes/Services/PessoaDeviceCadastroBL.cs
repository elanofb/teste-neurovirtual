using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using UTIL.Resources;

namespace BLL.Notificacoes {

    public class PessoaDeviceCadastroBL : DefaultBL, IPessoaDeviceCadastroBL {
	    
        //
        public PessoaDeviceCadastroBL() {
            
        }

        //
        public bool salvar(PessoaDevice OPessoaDevice) {

	        OPessoaDevice.idDevice = OPessoaDevice.idDevice.abreviar(100);
	        
	        OPessoaDevice.token = OPessoaDevice.token.abreviar(100);
	        
	        OPessoaDevice.versao = OPessoaDevice.versao.abreviar(100);
	        
	        OPessoaDevice.Pessoa = null;
	        
            if(OPessoaDevice.id > 0) {
	            return this.atualizar(OPessoaDevice);
            }
	        
			return this.inserir(OPessoaDevice);
        }

        //
        private bool inserir(PessoaDevice OPessoaDevice) {

            OPessoaDevice.setDefaultInsertValues();
	        
            db.PessoaDevice.Add(OPessoaDevice);
            
            db.SaveChanges();

            return OPessoaDevice.id > 0;
        }

        //
        private bool atualizar(PessoaDevice OPessoaDevice) {
            
            //Localizar existentes no banco
            var dbPessoaDevice = db.PessoaDevice.condicoesSeguranca().FirstOrDefault(x => x.id == OPessoaDevice.id);

            if (dbPessoaDevice == null) {
                return false;
            }

            //Atualizacao da Empresa
            var PessoaDeviceEntry = db.Entry(dbPessoaDevice);
	        
	        OPessoaDevice.setDefaultUpdateValues( );
	        
            PessoaDeviceEntry.CurrentValues.SetValues(OPessoaDevice);
	        
	        PessoaDeviceEntry.State = EntityState.Modified;
	        
            PessoaDeviceEntry.ignoreFields();
	        
            db.SaveChanges();
	        
            return OPessoaDevice.id > 0;
        }
	    
        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        
	        var retorno = new JsonMessageStatus();

            var OPessoaDevice = db.PessoaDevice.condicoesSeguranca().FirstOrDefault(x => x.id == id);

	        if (OPessoaDevice == null) {
	            
		        retorno.error = true;
	            
		        retorno.message = NotificationMessages.invalid_register_id;
		        
	        } else {
		        
		        OPessoaDevice.ativo = (OPessoaDevice.ativo != true);
		        
		        db.SaveChanges();
		        
		        retorno.active = OPessoaDevice.ativo == true ? "S" : "N";
		        
		        retorno.message = NotificationMessages.updateSuccess;    
	        }
	        
	        return retorno;
        }

    }
    
}