using System;
using System.Data.Entity;
using System.Linq;
using DAL.Contribuicoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Contribuicoes {

	public class ContribuicaoPrecoBL : TableRepository<ContribuicaoPreco>, IContribuicaoPrecoBL {

        //Atributos
	    private IContribuicaoPrecoDescontoBL _ContribuicaoPrecoDescontoBL;

        //Propriedades
        private IContribuicaoPrecoDescontoBL OContribuicaoPrecoDescontoBL => _ContribuicaoPrecoDescontoBL = _ContribuicaoPrecoDescontoBL ?? new ContribuicaoPrecoDescontoBL();

		//Carregamento de registro único pelo ID
		public ContribuicaoPreco carregar(int id) {
			var db = this.getDataContext();
			var query = from ContrPrec in db
									 .ContribuicaoPreco
									 .Include(x => x.TipoAssociado)
									 .Include(x => x.Contribuicao)
						where
						 ContrPrec.id == id &&
						 ContrPrec.TipoAssociado.flagExcluido == "N" &&
						 ContrPrec.flagExcluido == "N"
						select ContrPrec;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<ContribuicaoPreco> listar(int idContribuicao, int idTabelaPreco, string ativo) {

			var db = this.getDataContext();
			var query = from ContrPrec in db
				  .ContribuicaoPreco
				  .Include(x => x.TipoAssociado)
				  .Include(x => x.TipoAssociado.Categoria)
				  .Include(x => x.Contribuicao)
				  .AsNoTracking()
						where
						 ContrPrec.flagExcluido == "N"
                         && ContrPrec.Contribuicao.ativo == "S"
						select ContrPrec;

			if (idContribuicao > 0) {
				query = query.Where(x => x.idContribuicao == idContribuicao);
			}

			if (idTabelaPreco > 0) {
				query = query.Where(x => x.idTabelaPreco == idTabelaPreco);
			}


			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(ContribuicaoPreco OContribuicaoPreco) {

		    OContribuicaoPreco.Contribuicao = null;

		    OContribuicaoPreco.ContribuicaoTabelaPreco = null;

		    OContribuicaoPreco.ativo = "S";

		    if (OContribuicaoPreco.id == 0) {

		        return this.inserir(OContribuicaoPreco);

		    }

			this.atualizar(OContribuicaoPreco);

		    this.OContribuicaoPrecoDescontoBL.salvarLote(OContribuicaoPreco, OContribuicaoPreco.listaDesconto);

			return (OContribuicaoPreco.id > 0);

		}

        //Persistir e inserir um novo registro 
		//Inserir Contribuicao e lista de ContribuicaoPreco vinculados
        protected virtual bool inserir(ContribuicaoPreco OContribuicaoPreco) {

            var db = this.getDataContext();

			OContribuicaoPreco.setDefaultInsertValues();

            OContribuicaoPreco.listaDesconto.ForEach(Item => {

                Item.setDefaultInsertValues();
            });

			db.ContribuicaoPreco.Add(OContribuicaoPreco);

			db.SaveChanges();

			return OContribuicaoPreco.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Contribuicao e lista de ContribuicaoPreco
		protected virtual bool atualizar(ContribuicaoPreco OContribuicaoPreco) { 

			var db = this.getDataContext();

			//Localizar existentes no banco
			ContribuicaoPreco dbContribuicaoPreco = this.carregar(OContribuicaoPreco.id);

			//Configurar valores padrão
			OContribuicaoPreco.setDefaultUpdateValues();

    		//Atualizacao da Contribuição
			var RegistroEntry = db.Entry(dbContribuicaoPreco);

            RegistroEntry.CurrentValues.SetValues(OContribuicaoPreco);

            RegistroEntry.ignoreFields(new[] {"idTipoAssociado", "idTabelaPreco", "idContribuicao"});
	
			db.SaveChanges();

			return OContribuicaoPreco.id > 0;
		}

		//Excluir registro
		public UtilRetorno excluir(int id, int idUsuario) {

            var db = this.getDataContext();

			db.ContribuicaoPreco.Where(x => x.id == id)
			                    .Update( x => new ContribuicaoPreco { flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now});

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}