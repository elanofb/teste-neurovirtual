using System;
using System.Collections.Generic;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public class EnvioMensagemAtendimento : EnvioEmailAdapter, IEnvioMensagemAtendimento {
        
        //Private Construtor
	    private EnvioMensagemAtendimento(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioMensagemAtendimento factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioMensagemAtendimento(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

	    //Sobreposicao para designar corpo do e-mail
	    //Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
	    protected override string capturarConteudoHTML(string arquivoHTML) {

	        var OConfigNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(idOrganizacao);

	        string conteudoHTML = OConfigNotificacao.corpoEmailMensagemAtendimento;

	        string htmlMaster = this.capturarMasterpage("");

	        string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

	        htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

	        return htmlFinal;
	    }

	    //Sobreposicao obrigatorio do metodo abstrato
	    public override UtilRetorno enviar(IDictionary<string, object> info, string assunto = "Mensagem de Atendimento") {

	        this.Subject = assunto;

	        this.prepararMensagem();

	        this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

	        this.Body = this.Body.Replace("#NOME_PESSOA#", info["nomeDestinatario"].ToString());

	        this.Body = this.Body.Replace("#NRO_ATENDIMENTO#", info["nroAtendimento"].ToString());

	        this.Body = this.Body.Replace("#MENSAGEM#", info["mensagem"].ToString());

            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);

	        return this.disparar();
	    }

	    //
	    public UtilRetorno enviar(Atendimento OAtendimento, string mensagem) {

	        Dictionary<string, object> infos = new Dictionary<string, object>();

	        infos["nroAtendimento"] = OAtendimento.id;

	        infos["nomeDestinatario"] = OAtendimento.nome;

	        infos["mensagem"] = mensagem;

            var OConfigNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(idOrganizacao);

            string tituloEmail = OConfigNotificacao.assuntoEmailMensagemAtendimento.Replace("#NRO_ATENDIMENTO#", OAtendimento.id.ToString());

	        return this.enviar(infos, tituloEmail);
	    }
    }
}