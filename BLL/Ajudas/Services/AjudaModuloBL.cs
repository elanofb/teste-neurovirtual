using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Ajudas;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Ajudas {

	public class AjudaModuloBL : DefaultBL, IAjudaModuloBL {
		
        //
	    public IQueryable<AjudaModulo> query() {

	        var query = from Parc in db.AjudaModulo
	                    where Parc.flagExcluido == false
	                    select Parc;
            
            return query;

        }

		//Carregamento de registro único pelo ID
		public AjudaModulo carregar(int id) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);

		}
        
		//Listagem de Registros
		public IQueryable<AjudaModulo> listar(string valorBusca, bool? ativo) {
			
			var query = this.query().condicoesSeguranca()
                                    .Where(x => x.flagExcluido == false);

			if (!String.IsNullOrEmpty(valorBusca)) { 
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.titulo.Contains(valorBusca) || x.chamada.Contains(valorBusca));
			}

		    if (!ativo.isEmpty()) {
		        query = query.Where(x => x.ativo == ativo);
            }
            
			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		//Salvar o logotipo do AjudaModulo no banco de dados
		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(AjudaModulo OAjudaModulo) {

			bool flagSucesso = false;
			
		    if (OAjudaModulo.id > 0) { 
		        flagSucesso = this.atualizar(OAjudaModulo);
		    }

			if (OAjudaModulo.id == 0) { 
				flagSucesso = this.inserir(OAjudaModulo);
			}
            
			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(AjudaModulo OAjudaModulo) { 

			OAjudaModulo.setDefaultInsertValues();

            db.AjudaModulo.Add(OAjudaModulo);

            db.SaveChanges();

			return (OAjudaModulo.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(AjudaModulo OAjudaModulo) { 

			OAjudaModulo.setDefaultUpdateValues();

			//Localizar existentes no banco
			AjudaModulo dbAjudaModulo = this.carregar(OAjudaModulo.id);		

			var AjudaModuloEntry = db.Entry(dbAjudaModulo);

			AjudaModuloEntry.CurrentValues.SetValues(OAjudaModulo);

			AjudaModuloEntry.ignoreFields();

			db.SaveChanges();

			return (OAjudaModulo.id > 0);

		}

		// Verificar se já existe um registro com o mesmo nome, no entanto, que possua id diferente do informado
		public bool existe(string descricao, int id) {

			var query = from Parc in db.AjudaModulo
						where 
							Parc.descricao == descricao && 
							Parc.flagExcluido == false && 
							Parc.id != id 
						select Parc;
			var OItem = query.Take(1).FirstOrDefault();
			return (OItem != null);
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

        //Exclusão logica de registros
        public UtilRetorno excluir(int id) {

			AjudaModulo OAjudaModulo = this.carregar(id);

			if (OAjudaModulo == null) {
				return UtilRetorno.newInstance(true, "O registro não foi localizado.");
			}

			OAjudaModulo.flagExcluido = true;
			OAjudaModulo.idUsuarioAlteracao = User.id();
			OAjudaModulo.dtAlteracao = DateTime.Now;
			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}