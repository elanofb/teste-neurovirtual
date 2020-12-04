using System;
using System.Data.Entity;
using System.Linq;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using DAL.Planos;
using BLL.Core.Events;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.Planos {

    public class PlanoContratacaoBL: TableRepository<PlanoContratacao>, IPlanoContratacaoBL {

        //Constantes

        //Atributos

        //Propriedades

        //Events
		private EventAggregator onPlanoContratado = OnPlanoContratado.getInstance;

        //Construtor
		public PlanoContratacaoBL() {
            this.onPlanoContratado.subscribe(new OnPlanoContratadoHandler() );
        }

		//Carregar registro a partir do ID
		public PlanoContratacao carregar(int id) {
            
            var db = this.getDataContext();

			var query = from N in db.PlanoContratacao
                        .Include(x => x.Plano)
                        .Include(x => x.Pessoa)
                        .Include(x => x.Status)
						where 
							N.flagExcluido == "N" && 
							N.id == id
						select N;

			return query.FirstOrDefault();
		}
		
        //Listagem de registros conforme filtros
		public IQueryable<PlanoContratacao> listar(string valorBusca, int idPlano = 0,  int idStatus = 0) {
			
            var db = this.getDataContext();

			var query = from N in db.PlanoContratacao
                        .Include(x => x.Pessoa)
                        .Include(x => x.Plano)
                        .Include(x => x.Status)
						where N.flagExcluido == "N"
						select N;

			if (idStatus > 0) {
				query = query.Where(x => x.idStatus == idStatus);
			}

			if (idPlano > 0) {
				query = query.Where(x => x.idPlano == idPlano);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca));
			}


			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(PlanoContratacao OPlanoContratacao) {

			bool flagSucesso = false;

			if(OPlanoContratacao.id == 0){	
				flagSucesso = this.inserir(OPlanoContratacao);
                //if(flagSucesso) this.onPlanoContratado.publish( (OPlanoContratacao as object) );
			}

			flagSucesso = this.atualizar(OPlanoContratacao);            

			return flagSucesso;
		}

        //Persistir e inserir um novo registro 
        public bool inserir(PlanoContratacao OPlanoContratacao) { 
            var db = this.getDataContext();

			OPlanoContratacao.setDefaultInsertValues<PlanoContratacao>();
			
			db.PlanoContratacao.Add(OPlanoContratacao);
			db.SaveChanges();

			return OPlanoContratacao.id > 0;
		}

        //Persistir e atualizar um registro existente 
		public bool atualizar(PlanoContratacao OPlanoContratacao) { 

            var db = this.getDataContext();

			//Localizar existentes no banco
			PlanoContratacao dbOcorrenciaRelacionamento = this.carregar(OPlanoContratacao.id);

			//Configurar valores padrão
			OPlanoContratacao.setDefaultUpdateValues<PlanoContratacao>();

			//Atualizacao da Instituicao
			var PlanoContratacaoEntry = db.Entry(dbOcorrenciaRelacionamento);
			PlanoContratacaoEntry.CurrentValues.SetValues(OPlanoContratacao);
			PlanoContratacaoEntry.ignoreFields<PlanoContratacao>();

			db.SaveChanges();
			return OPlanoContratacao.id > 0;
		}

		//Exclusao Logica
		public UtilRetorno excluir(int id) {

            var db = this.getDataContext();

			PlanoContratacao OPlanoContratacao = this.carregar(id);

			if (OPlanoContratacao == null) { 
				return UtilRetorno.newInstance(true, "O registro não pode ser localizado.");
			}

			OPlanoContratacao.flagExcluido = "S";
			OPlanoContratacao.dtAlteracao = DateTime.Now; 

            db.SaveChanges();

            //Excluo tambem o titulo de cobrança
			//ITituloReceitaBL OTituloReceitaPlanoContratacaoBL = new TituloReceitaPlanoContratacaoBL();
   //         int idMacroConta = (int)MacroContaEnum.PLANO;
            //OTituloReceitaPlanoContratacaoBL.excluir(idMacroConta, id);


			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}

        //Verificar se já existe um registro com o documento/email informado, que possua id diferente do informado
        public bool existe(int idPessoa, int idPlano) {

            var db = this.getDataContext();

			var query = from N in db.PlanoContratacao
						where 
							N.flagExcluido == "N" && 
							N.idPessoa == idPessoa &&
                            N.idPlano == idPlano &&
                            N.idStatus != StatusPlanoContratacaoConst.ENCERRADO
						select N;

            var OPlanoContratacao = query.Take(1).FirstOrDefault();
            return (OPlanoContratacao == null ? false : true);
        }

		public bool atualizarStatus(int id, int status) {

            var db = this.getDataContext();

			this.getDataContext().PlanoContratacao.Where(x => x.id == id)
				.Update(x => new PlanoContratacao { idStatus = status });
            
			var listaCheck = this.getDataContext().PlanoContratacao.Where(x => x.id == id && x.idStatus == status).ToList();
			return (listaCheck.Count == 0);
		}
	}
}