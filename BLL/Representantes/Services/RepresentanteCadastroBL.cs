using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Pessoas;
using DAL.Representantes;
using UTIL.Resources;

namespace BLL.Representantes {

	public class RepresentanteCadastroBL : DefaultBL , IRepresentanteCadastroBL {

		//
		public RepresentanteCadastroBL() {
		}
		
	
        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(Representante ORepresentante){
			
            if (ORepresentante.id == 0) {
                return this.inserir(ORepresentante);
            }
			
            return this.atualizar(ORepresentante);
		}

        //Persistir o objecto e salvar na base de dados
        private bool inserir(Representante ORepresentante) {
            
            ORepresentante.setDefaultInsertValues();
            ORepresentante.Pessoa.setDefaultInsertValues();
	        
            db.Representante.Add(ORepresentante);
            db.SaveChanges();
	        
            return (ORepresentante.id > 0);
	        
        }
		
        //Persistir o objecto e atualizar informações
        private bool atualizar(Representante ORepresentante){

            Representante dbRepresentante = db.Representante.Include(x => x.Pessoa).FirstOrDefault(x => x.id == ORepresentante.id && x.dtExclusao == null);
            
            if (dbRepresentante == null){
                return false;
            }
			
	        var RepresentanteEntry = db.Entry(dbRepresentante);
	        ORepresentante.setDefaultUpdateValues();
	        RepresentanteEntry.CurrentValues.SetValues(ORepresentante);
	        RepresentanteEntry.State = EntityState.Modified;
	        RepresentanteEntry.ignoreFields(new [] { "idPessoa", "ativo" });
	        
	        var PessoaEntry = db.Entry(dbRepresentante.Pessoa);
	        ORepresentante.Pessoa.setDefaultUpdateValues();
	        ORepresentante.Pessoa.id = dbRepresentante.Pessoa.id;
	        PessoaEntry.CurrentValues.SetValues(ORepresentante.Pessoa);
	        PessoaEntry.State = EntityState.Modified;
	        PessoaEntry.ignoreFields();
	        
            db.SaveChanges();
            return (ORepresentante.id > 0);
	        
        }
		
		//Alteracao de status
		public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = db.Representante.FirstOrDefault(x => x.id == id && x.dtExclusao == null);
			
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