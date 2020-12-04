using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public class ContribuicaoCobrancaBL : DefaultBL, IContribuicaoCobrancaBL {

        //Atributos

        //Propriedades

		//Carregamento de registro único pelo ID
		public ContribuicaoCobranca carregar(int id) {
			
			var query = from ContCob in db
									 .ContribuicaoCobranca
									 .Include(x => x.Contribuicao)
									 .Include(x => x.UsuarioCobranca)
						where
						 ContCob.id == id
						select ContCob;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<ContribuicaoCobranca> listar(int idContribuicao) {

			var query = from ContCob in db
				        .ContribuicaoCobranca
						.Include(x => x.Contribuicao)
						.Include(x => x.UsuarioCobranca)
				  .AsNoTracking()
					select ContCob;

			if (idContribuicao > 0) {
				query = query.Where(x => x.idContribuicao == idContribuicao);
			}

    		return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(ContribuicaoCobranca OContribuicaoCobranca) {

		    OContribuicaoCobranca.Contribuicao = null;


		    if (OContribuicaoCobranca.id == 0) {

		        return this.inserir(OContribuicaoCobranca);

		    }

			this.atualizar(OContribuicaoCobranca);

			return (OContribuicaoCobranca.id > 0);

		}

        //Persistir e inserir um novo registro 
		//Inserir Contribuicao e lista de ContribuicaoPreco vinculados
        protected virtual bool inserir(ContribuicaoCobranca OContribuicaoCobranca) {

			OContribuicaoCobranca.setDefaultInsertValues();

			db.ContribuicaoCobranca.Add(OContribuicaoCobranca);

			db.SaveChanges();

			return OContribuicaoCobranca.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Contribuicao e lista de ContribuicaoPreco
		protected virtual bool atualizar(ContribuicaoCobranca OContribuicaoCobranca) { 

			//Localizar existentes no banco
			ContribuicaoCobranca dbContribuicaoCobranca = this.carregar(OContribuicaoCobranca.id);

			//Configurar valores padrão
			OContribuicaoCobranca.setDefaultUpdateValues();

    		//Atualizacao da Contribuição
			var RegistroEntry = db.Entry(dbContribuicaoCobranca);

            RegistroEntry.CurrentValues.SetValues(OContribuicaoCobranca);

            RegistroEntry.ignoreFields(new[] {"idContribuicao"});
	
			db.SaveChanges();

			return OContribuicaoCobranca.id > 0;
		}


	}
}