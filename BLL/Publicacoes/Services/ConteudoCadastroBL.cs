using System;
using System.Json;
using DAL.Publicacoes;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public class ConteudoCadastroBL : ConteudoConsultaBL , IConteudoCadastroBL {

		//
		public ConteudoCadastroBL() {
		}

        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(Conteudo OConteudo){
			
			OConteudo.titulo = OConteudo.titulo.abreviar(255);
			OConteudo.idInterno = OConteudo.idInterno.abreviar(50);
				
            if (OConteudo.id == 0) {
                return this.inserir(OConteudo);
            }
			
            return this.atualizar(OConteudo);
		}

        //Persistir o objecto e salvar na base de dados
        private bool inserir(Conteudo OConteudo) {
            
            OConteudo.setDefaultInsertValues<Conteudo>();
            db.Conteudo.Add(OConteudo);
            db.SaveChanges();
            return (OConteudo.id > 0);
        }
		
        //Persistir o objecto e atualizar informações
        private bool atualizar(Conteudo OConteudo){

            Conteudo dbConteudo = this.carregar(OConteudo.id);
			
            if (dbConteudo == null){
                return false;
            }
			
            var tipoEntry = db.Entry(dbConteudo);
			
            OConteudo.setDefaultUpdateValues<Conteudo>();
            tipoEntry.CurrentValues.SetValues(OConteudo);
            tipoEntry.ignoreFields<Conteudo>(new [] { "ativo" });
			
            db.SaveChanges();
            return (OConteudo.id > 0);
	        
        }
		
		//Alteracao de status
		public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);
			
			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo != true);
				db.SaveChanges();
				retorno.active = item.ativo ? "S" : "N";
				retorno.message = NotificationMessages.updateSuccess;
			}
			return retorno;
		}

      
	}
}