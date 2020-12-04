using System;
using System.Data.Entity;
using System.Linq;
using BLL.Configuracoes;
using DAL.Contribuicoes;
using BLL.Services;
using DAL.Configuracoes;

namespace BLL.Contribuicoes {

	public class ContribuicaoAtualBL : DefaultBL, IContribuicaoAtualBL{

        //Atributos

        //Propriedades


		//Carregameno da ultima contribuicao do sistema
	    public Contribuicao carregar() {

            ConfiguracaoContribuicao OConfiguracaoContribuicao = ConfiguracaoContribuicaoBL.getInstance.carregar();

	        Contribuicao Contribuicao = new Contribuicao();

	        //if (OConfiguracaoContribuicao.idTipoContribuicaoAtual == TipoContribuicaoConst.ANUIDADES) {

	        //    Contribuicao = this.carregarAnuidade((short) DateTime.Today.Year);

	        //} else {

         //       Contribuicao = this.carregarMensalidade((short)DateTime.Today.Month, (short)DateTime.Today.Year);

         //   }

	        return Contribuicao;
	    }

        //
	    public Contribuicao carregarAnuidade(short ano = 0) {
			
            int idTipoContribuicao = TipoContribuicaoConst.ANUIDADES;

			var query = from Cont in this.db
									.Contribuicao
									.Include(x => x.TipoContribuicao)
						where	
							Cont.dtCancelamento == null &&
                            Cont.ativo == "S" &&
                            Cont.idTipoContribuicao == idTipoContribuicao

						select Cont;

			if (ano > 0) {
				query = query.Where(x => x.anoInicioVigencia == ano);
			}

			Contribuicao Retorno = query.OrderByDescending(x => x.id).FirstOrDefault();

			return Retorno;
		}

		//Carregameno da ultima contribuicao do sistema
		public Contribuicao carregarMensalidade(int mes = 0, short ano = 0) {
			
            DateTime dtInicioVigencia = new DateTime(ano, mes, 1, 00, 00, 00);

            int idTipoContribuicao = TipoContribuicaoConst.MENSALIDADES;

			var query = from Cont in this.db
									.Contribuicao
									.Include(x => x.TipoContribuicao)
						where	
							Cont.dtCancelamento == null &&
                            Cont.ativo == "S" &&
                            Cont.idTipoContribuicao == idTipoContribuicao &&
                            dtInicioVigencia >= Cont.dtInicioVigencia
                            
						select Cont;

			Contribuicao Retorno = query
                                    .OrderByDescending(x => x.dtInicioVigencia)
                                    .FirstOrDefault();

			return Retorno;
		}
	}
}