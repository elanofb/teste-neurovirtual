using BLL.Core.Events;
using DAL.Associados;
using System;
using BLL.Configuracoes;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Configuracoes;
using DAL.Contribuicoes;
using DAL.Relacionamentos;

namespace BLL.NaoAssociados.Events {

	public class OnTornarAssociadoHandler : IHandler<object> {

		//Atributos
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propridades
		private Associado OAssociado { get; set;}
		private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL() );


	    //Chamador das ações do evento
		public void execute(object source) {
			try {

				this.OAssociado = (source as Associado);

				this.gerarOcorrencia();


			} catch (Exception ex) {
				UtilLog.saveError(ex, "");
			}
		}

		//Gerar Ocorrencia para histórico do associado
		public void gerarOcorrencia() { 
			PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();
			Ocorrencia.dtOcorrencia = DateTime.Now;
			Ocorrencia.idPessoa = OAssociado.idPessoa;
			Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idTornarAssociado;
			Ocorrencia.observacao = this.OAssociado.observacoes;

			this.OPessoaRelacionamentoBL.salvar(Ocorrencia);
		}

        // Vincular contribuição ao Associado
        private void vincularContribuicao() {

	        try {
                ConfiguracaoContribuicao OConfiguracaoContribuicao = ConfiguracaoContribuicaoBL.getInstance.carregar();

	            var Contribuicao = new Contribuicao();

	            //if (OConfiguracaoContribuicao.idTipoContribuicaoAtual == TipoContribuicaoConst.ANUIDADES) {
	            //    Contribuicao = this.OContribuicaoAtualBL.carregarAnuidade((short) DateTime.Today.Year);
	            //} else {
             //       Contribuicao = this.OContribuicaoAtualBL.carregarMensalidade((short)DateTime.Today.Month, (short)DateTime.Today.Year);
             //   }

                if (Contribuicao == null) {
			        return;
		        }

		  //      AssociadoContribuicao OAssociadoContribuicao = new AssociadoContribuicao();

		  //      OAssociadoContribuicao.Associado = this.OAssociado;
		        
				//OAssociadoContribuicao.idAssociado = this.OAssociado.id;
		        
				//OAssociadoContribuicao.idTipoAssociado = UtilNumber.toInt32(this.OAssociado.idTipoAssociado);
		        
				//OAssociadoContribuicao.Contribuicao = Contribuicao;
		        
				//OAssociadoContribuicao.idContribuicao = Contribuicao.id;
		        
				//OAssociadoContribuicao.valorOriginal = Contribuicao.retornarValorVigente(UtilNumber.toInt32(this.OAssociado.idTipoAssociado), true);
		        
				//OAssociadoContribuicao.valorAtual = OAssociadoContribuicao.valorOriginal;
		        
				//OAssociadoContribuicao.dtVencimentoOriginal = Contribuicao.retornarVencimentoVigente(UtilNumber.toInt32(this.OAssociado.idTipoAssociado), OConfiguracaoContribuicao);
		        
				//OAssociadoContribuicao.dtVencimentoAtual = OAssociadoContribuicao.dtVencimentoOriginal;

                //if(OConfiguracaoContribuicao.idTipoContribuicaoAtual == TipoContribuicaoConst.ANUIDADES) {
                //    this.OAssociadoAnuidadeBL.salvar(OAssociadoContribuicao);
                //} else {
                //    this.OAssociadoMensalidadeBL.salvar(OAssociadoContribuicao);
                //}

	        } catch (Exception ex) {
		        UtilLog.saveError(ex, String.Format("Erro ao vincular uma anuidade para o associado {0}", this.OAssociado.Pessoa.nome));
	        }
        }
	}
}