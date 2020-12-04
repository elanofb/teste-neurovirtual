using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System.Collections.Generic;
using System;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Relacionamentos;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Configuracoes;
using BLL.NaoAssociados;

namespace BLL.NaoAssociadosInstitucional.Events {

	public class OnNaoAssociadoInstitucionalCadastradoHandler : IHandler<object> {

		//Atributos
		private INaoAssociadoBL _NaoAssociadoBL;
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propridades
		private Associado OAssociado { get; set;}
		private INaoAssociadoBL ONaoAssociadoBL => (this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL() );
	    private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL() );

	    //Chamador das ações do evento
		public void execute(object source) {
			try {

				var NovoAssociado = (source as Associado);

				this.OAssociado = this.ONaoAssociadoBL.carregar(NovoAssociado.id).FirstOrDefault();

				this.gerarOcorrencia();

				this.dispararEmail();

			} catch (Exception ex) {
				UtilLog.saveError(ex, String.Format("Erro no manipulador do evento de cadastro do associado {0}", this.OAssociado.Pessoa.nome));
			}
		}

		//Gerar Ocorrencia para histórico do associado
	    private void gerarOcorrencia() { 
			PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();
			Ocorrencia.dtOcorrencia = DateTime.Now;
			Ocorrencia.idPessoa = OAssociado.idPessoa;
			Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idRealizacaoCadastro;
			Ocorrencia.observacao = this.OAssociado.observacoes;

			this.OPessoaRelacionamentoBL.salvar(Ocorrencia);
		}

	    private void dispararEmail() { 

            ConfiguracaoNotificacao OConfiguracaoNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.OAssociado.idOrganizacao);

	        if (String.IsNullOrEmpty(OConfiguracaoNotificacao.emailNovoNaoAssociado)) {
		        return;
	        }

            List<string> listaEmail = OConfiguracaoNotificacao.emailNovoNaoAssociado.Split(';').ToList();

			var OEmail = EnvioNovoNaoAssociado.factory(OAssociado.idOrganizacao, this.OAssociado.Pessoa.ToEmailList(), null, listaEmail);
            
			OEmail.enviar(this.OAssociado);
        }
	}
}