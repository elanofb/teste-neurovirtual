using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System.Collections.Generic;
using System;
using BLL.Associados;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Relacionamentos;
using BLL.Contribuicoes;
using BLL.AssociadosContribuicoes;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Configuracoes;

namespace BLL.AssociadosInstitucional.Events {

	public class OnAssociadoInstitucionalCadastradoHandler : IHandler<object> {

		//Atributos
		private IAssociadoBL _AssociadoBL;
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;


		//Propridades
		private Associado OAssociado { get; set;}
		private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
	    private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL() );


		//Chamador das ações do evento
		public void execute(object source) {
			try {

				var NovoAssociado = (source as Associado);

				this.OAssociado = this.OAssociadoBL.carregar(NovoAssociado.id);

				this.gerarOcorrencia();

				this.dispararEmail();

			} catch (Exception ex) {
				UtilLog.saveError(ex, $"Erro no manipulador do evento de cadastro do associado {this.OAssociado.Pessoa.nome}");
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

        //Enviar e-mail para o associado após o cadastro
	    private void dispararEmail() { 

            ConfiguracaoNotificacao OConfiguracaoNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(OAssociado.idOrganizacao);

	        if (String.IsNullOrEmpty(OConfiguracaoNotificacao.emailNovoAssociado)) {
		        return;
	        }

            List<string> listaEmail = OConfiguracaoNotificacao.emailNovoAssociado.Split(';').ToList();

			var OEmail = EnvioNovoAssociado.factory(OAssociado.idOrganizacao, this.OAssociado.Pessoa.ToEmailList(), null, listaEmail);
            
			OEmail.enviar(this.OAssociado);
        }
	}
}