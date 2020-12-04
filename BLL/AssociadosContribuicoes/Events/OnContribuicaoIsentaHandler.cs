using BLL.Core.Events;
using System;
using BLL.Associados;
using DAL.AssociadosContribuicoes;
using BLL.Financeiro;
using DAL.Associados;
using DAL.Pessoas;
using DAL.Relacionamentos;
using DAL.Financeiro;

namespace BLL.AssociadosContribuicoes.Events {

	public class OnContribuicaoIsentaHandler : IHandler<object> {

		//Atributos
        private IAssociadoBL _AssociadoBL;
        private IAssociadoRelacionamentoBL _AssociadoRelacionamentoBL;
        private IAssociadoAcaoBL _AssociadoAcaoBL;

		//Propridades
        private IAssociadoBL OAssociadoBL => (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL() );
	    private IAssociadoRelacionamentoBL OAssociadoRelacionamentoBL => (this._AssociadoRelacionamentoBL = this._AssociadoRelacionamentoBL ?? new AssociadoRelacionamentoBL() );
	    private IAssociadoAcaoBL OAssociadoAcaoBL => (this._AssociadoAcaoBL = this._AssociadoAcaoBL ?? new AssociadoAcaoBL() );

	    //Chamador das ações do evento
		public void execute(object source) {

			try {

				var OContribuicaoIsentada = (source as AssociadoContribuicao);

				this.excluirTituloFinanceiro(OContribuicaoIsentada);

                this.salvarHistorico(OContribuicaoIsentada);

			    this.OAssociadoAcaoBL.atualizarUltimoPagamentoContribuicao(OContribuicaoIsentada);

			} catch (Exception ex) {
				UtilLog.saveError(ex, "Erro no manipulador de evento: OnContribuicaoIsentadaHandler");
			}
		}

		//Exclui o título
		private void excluirTituloFinanceiro(AssociadoContribuicao OAssociadoContribuicao) { 

            var OTituloReceitaExclusaoBL = new TituloReceitaExclusaoBL();

            OTituloReceitaExclusaoBL.excluir(TipoReceitaConst.CONTRIBUICAO, OAssociadoContribuicao.id, "Contribuicao isenta");

		}

		//Salva o histórico
		private void salvarHistorico(AssociadoContribuicao OAssociadoContribuicao) { 

			Associado OAssociado = this.OAssociadoBL.carregar(OAssociadoContribuicao.idAssociado);

			PessoaRelacionamento Relacionamento = new PessoaRelacionamento();

			Relacionamento.idPessoa = OAssociado.idPessoa;

            Relacionamento.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idIsencaoContribuicao;

            Relacionamento.observacao = String.Concat("Isenção ", OAssociadoContribuicao.Contribuicao.descricao, ", motivo: ", OAssociadoContribuicao.motivoIsencao);

            Relacionamento.dtOcorrencia = DateTime.Now;

			this.OAssociadoRelacionamentoBL.salvar(Relacionamento);
		}
	}
}