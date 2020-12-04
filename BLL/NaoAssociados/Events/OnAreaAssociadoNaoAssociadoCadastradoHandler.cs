using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System.Collections.Generic;
using System;
using BLL.Associados;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Pessoas;
using BLL.Pessoas;
using BLL.Services;
using DAL.Relacionamentos;
using DAL.Configuracoes;

namespace BLL.NaoAssociados.Events {

	public class OnAreaAssociadoNaoAssociadoCadastradoHandler : DefaultBL, IHandler<object> {

		//Atributos
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;
        private ITipoAssociadoBL _TipoAssociadoBL;

        //Propridades
        private Associado OAssociado { get; set;}

        private TipoAssociado OTipoAssociado { get; set;}


        private ITipoAssociadoBL OTipoAssociadoBL => this._TipoAssociadoBL = this._TipoAssociadoBL ?? new TipoAssociadoBL();

        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL());


        //Chamador das ações do evento
        public void execute(object source) {
			try {

				this.OAssociado = (source as Associado);

    	        this.OTipoAssociado = this.OTipoAssociadoBL.carregar(UtilNumber.toInt32(OAssociado.idTipoAssociado));

                this.gerarOcorrencia();

				this.dispararEmail();


            } catch (Exception ex) {
				UtilLog.saveError(ex,$"Erro no manipulador do evento de cadastro do associado {this.OAssociado.Pessoa.nome}");
			}
		}

		//Gerar Ocorrencia para histórico do associado
		private void gerarOcorrencia() {

		    try {

                PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();

                Ocorrencia.dtOcorrencia = DateTime.Now;

                Ocorrencia.idPessoa = OAssociado.idPessoa;

                Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idRealizacaoCadastro;

                Ocorrencia.observacao = this.OAssociado.observacoes;

			    this.OPessoaRelacionamentoBL.salvar(Ocorrencia);

		    } catch (Exception ex) {
		        UtilLog.saveError(ex, "Erro ao salvar ocorrência após cadastro de associado pelo sistema");
		    }
		}

        //
        private void dispararEmail() {

            try {
			    if (OAssociado.dtImportacao.HasValue) {
			        return;
			    }

                ConfiguracaoNotificacao OConfiguracaoNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(OAssociado.idOrganizacao);

	            if (String.IsNullOrEmpty(OConfiguracaoNotificacao.emailNovoAssociado)) {
		            return;
	            }

                List<string> listaEmail = OConfiguracaoNotificacao.emailNovoAssociado.Split(';').ToList();

			    var OEmail = EnvioNovoNaoAssociado.factory(OAssociado.idOrganizacao, this.OAssociado.Pessoa.ToEmailList(), null, listaEmail);
            
			    OEmail.enviar(this.OAssociado);

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro ao enviar e-mail após cadastro de associado pelo sistema");
            }
        }

    }
}