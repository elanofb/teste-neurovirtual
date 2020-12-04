using System;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public class ContribuicaoPadraoBL : ContribuicaoBL {

		//Atributos
	    private IContribuicaoVencimentoBL _ContribuicaoVencimentoBL;

		//Propriedades
	    private IContribuicaoVencimentoBL OContribuicaoVencimentoBL=> _ContribuicaoVencimentoBL = _ContribuicaoVencimentoBL ?? new ContribuicaoVencimentoBL();

		//Listagem de Registros
		public override IQueryable<Contribuicao> listar(string valorBusca, string ativo) {

			var query = base.listar(valorBusca, ativo);
			
			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
        public override bool salvar(Contribuicao OContribuicao) {

            bool flagSucesso;

            if(OContribuicao.id == 0){
                	
				flagSucesso = this.inserir(OContribuicao);

                this.OContribuicaoVencimentoBL.salvarLote(OContribuicao, OContribuicao.listaContribuicaoVencimento);

                return flagSucesso;
            }

            flagSucesso = this.atualizar(OContribuicao);

            return flagSucesso;
        }

        //Persistir e inserir um novo registro 
		//Inserir Contribuicao e lista de ContribuicaoPreco vinculados
        protected override bool inserir(Contribuicao OContribuicao) {

			OContribuicao.setDefaultInsertValues();

            OContribuicao.listaContribuicaoVencimento.ForEach(Item => {
                Item.setDefaultInsertValues();
            });

            OContribuicao.listaContribuicaoPreco = null;

            OContribuicao.PeriodoContribuicao = null;

            OContribuicao.TipoGeracaoContribuicao = null;

            OContribuicao.TipoVencimento = null;

            OContribuicao.CentroCusto = null;

            OContribuicao.MacroConta = null;

            OContribuicao.CategoriaTitulo = null;

            OContribuicao.ContaBancaria = null;

			db.Contribuicao.Add(OContribuicao);

			db.SaveChanges();

			return OContribuicao.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Contribuicao e lista de ContribuicaoPreco
		protected override bool atualizar(Contribuicao OContribuicao) { 

			//Localizar existentes no banco
			Contribuicao dbContribuicao = this.carregar(OContribuicao.id);

			//Configurar valores padrão
			OContribuicao.setDefaultUpdateValues();

			//Atualizacao da Contribuição
			var ContribuicaoEntry = db.Entry(dbContribuicao);
			ContribuicaoEntry.CurrentValues.SetValues(OContribuicao);
			ContribuicaoEntry.ignoreFields(new[]{"idPeriodoContribuicao", "idTipoVencimento"});
	
			db.SaveChanges();
			return OContribuicao.id > 0;
		}

		//Verificar se já existe uma anuidade com para o ano informado
        public override bool existe(Contribuicao OContribuicao) {

			var query = from Contr in db.Contribuicao.AsNoTracking()
						where
							Contr.id != OContribuicao.id && 
							Contr.anoInicioVigencia == OContribuicao.anoInicioVigencia &&
							Contr.dtCancelamento == null
						select 
							Contr;

		    query = query.condicoesSeguranca();

			bool flagExiste = query.Any();

            return flagExiste;
        }
    }
}
