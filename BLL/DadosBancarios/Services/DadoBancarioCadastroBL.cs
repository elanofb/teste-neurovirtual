using System;
using System.Json;
using BLL.DadosBancarios.Interfaces;
using DAL.DadosBancarios;
using UTIL.Resources;

namespace BLL.DadosBancarios.Services {
    
    public class DadoBancarioCadastroBL : DadoBancarioConsultaBL, IDadoBancarioCadastroBL{
        
        public bool salvar(DadoBancario ODadoBancario){

            ODadoBancario.Pessoa = null;
            ODadoBancario.Banco = null;
            
            ODadoBancario.nroAgencia = UtilString.onlyAlphaNumber(ODadoBancario.nroAgencia.abreviar(10));
            ODadoBancario.nroConta = UtilString.onlyAlphaNumber(ODadoBancario.nroConta.abreviar(20));
            ODadoBancario.digitoConta = ODadoBancario.digitoConta.abreviar(5);
            ODadoBancario.nomeTitular = ODadoBancario.nomeTitular.abreviar(50);
            ODadoBancario.documentoTitular = UtilString.onlyAlphaNumber(ODadoBancario.documentoTitular.abreviar(14));
            ODadoBancario.observacoes = ODadoBancario.observacoes.abreviar(255);
            
            if (ODadoBancario.id == 0) {
                return this.inserir(ODadoBancario);
            }

            return this.atualizar(ODadoBancario);
        }
        
        private bool inserir(DadoBancario ODadoBancario) {
        
            ODadoBancario.setDefaultInsertValues();            
            db.DadoBancario.Add(ODadoBancario);
            db.SaveChanges();
            
            return (ODadoBancario.id > 0);
        }

        private bool atualizar(DadoBancario ODadoBancario) {

            ODadoBancario.setDefaultUpdateValues();
            var dbDadoBancario = this.carregar(ODadoBancario.id);
            
            if (dbDadoBancario == null) {
                return false;
            }
            
            var ItemEntry = db.Entry(dbDadoBancario);
            ItemEntry.CurrentValues.SetValues(ODadoBancario);
            ItemEntry.ignoreFields();
                
            db.SaveChanges();
            
            return (ODadoBancario.id > 0);
        }
        
        /// <summary>
        /// Ativar ou desativar um registro
        /// </summary>
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo != true);
                db.SaveChanges();
                retorno.active = item.ativo == true ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }
                
    }
}