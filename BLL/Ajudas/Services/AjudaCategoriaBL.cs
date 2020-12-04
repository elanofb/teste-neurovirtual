using System;
using System.Linq;
using BLL.Services;
using DAL.Ajudas;
using DAL.Permissao.Security.Extensions;

namespace BLL.Ajudas {

	public class AjudaCategoriaBL : DefaultBL, IAjudaCategoriaBL {
		
        //
	    public IQueryable<AjudaCategoria> query() {

	        var query = from Parc in db.AjudaCategoria
	                    where Parc.flagExcluido == false
	                    select Parc;
            
            return query;

        }

		//Carregamento de registro único pelo ID
		public AjudaCategoria carregar(int id) {

			var query = this.query().condicoesSeguranca();

			return query.FirstOrDefault(x => x.id == id);

		}
        
		//Listagem de Registros
		public IQueryable<AjudaCategoria> listar(string valorBusca, bool? ativo) {
			
			var query = this.query().condicoesSeguranca()
                                    .Where(x => x.flagExcluido == false);

			if (!String.IsNullOrEmpty(valorBusca)) { 
				query = query.Where(x => x.descricao.Contains(valorBusca) );
			}

		    if (!ativo.isEmpty()) {
		        query = query.Where(x => x.ativo == ativo);
            }
            
			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		//Salvar o logotipo do AjudaCategoria no banco de dados
		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(AjudaCategoria OAjudaCategoria) {

			bool flagSucesso = false;
			
		    if (OAjudaCategoria.id > 0) { 
		        flagSucesso = this.atualizar(OAjudaCategoria);
		    }

			if (OAjudaCategoria.id == 0) { 
				flagSucesso = this.inserir(OAjudaCategoria);
			}
            
			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(AjudaCategoria OAjudaCategoria) { 

			OAjudaCategoria.setDefaultInsertValues();

            db.AjudaCategoria.Add(OAjudaCategoria);

            db.SaveChanges();

			return (OAjudaCategoria.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(AjudaCategoria OAjudaCategoria) { 

			OAjudaCategoria.setDefaultUpdateValues();

			//Localizar existentes no banco
			AjudaCategoria dbAjudaCategoria = this.carregar(OAjudaCategoria.id);		

			var AjudaCategoriaEntry = db.Entry(dbAjudaCategoria);

			AjudaCategoriaEntry.CurrentValues.SetValues(OAjudaCategoria);

			AjudaCategoriaEntry.ignoreFields();

			db.SaveChanges();

			return (OAjudaCategoria.id > 0);

		}

		// Verificar se já existe um registro com o mesmo nome, no entanto, que possua id diferente do informado
		public bool existe(string descricao, int id) {

			var query = from Parc in db.AjudaCategoria
						where 
							Parc.descricao == descricao && 
							Parc.flagExcluido == false && 
							Parc.id != id 
						select Parc;
			var OItem = query.Take(1).FirstOrDefault();
			return (OItem != null);
		}

		//Exclusão logica de registros
		public UtilRetorno excluir(int id) {

			AjudaCategoria OAjudaCategoria = this.carregar(id);

			if (OAjudaCategoria == null) {
				return UtilRetorno.newInstance(true, "O registro não foi localizado.");
			}

			OAjudaCategoria.flagExcluido = true;
			OAjudaCategoria.idUsuarioAlteracao = User.id();
			OAjudaCategoria.dtAlteracao = DateTime.Now;
			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}