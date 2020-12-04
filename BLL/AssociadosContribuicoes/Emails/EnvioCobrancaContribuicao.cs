using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosContribuicoes.Emails;
using BLL.Configuracoes;
using BLL.Email;
using DAL.AssociadosContribuicoes;
using DAL.Notificacoes;

namespace BLL.AssociadosContribuicoes {

	public class EnvioCobrancaContribuicao : EnvioEmailAdapter, IEnvioCobrancaContribuicao {

        //Private

        //Propriedades
	    private AssociadoContribuicaoResumoVW AssociadoContribuicao;
        private NotificacaoSistema ONotificacao;

		//Private Construtor
		private EnvioCobrancaContribuicao(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioCobrancaContribuicao factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 

			return new EnvioCobrancaContribuicao(idOrganizacaoParam, listaDestino, listaCopia, null);

		}

		//
		public UtilRetorno enviar(NotificacaoSistema ONotificacao, AssociadoContribuicaoResumoVW OAssociadoContribuicao) {

		    this.AssociadoContribuicao = OAssociadoContribuicao;

            this.ONotificacao = ONotificacao;

		    string urlPagamento = UtilConfig.linkPgtoTitulo(OAssociadoContribuicao.idTituloReceita.toInt());

			Dictionary<string, object> infos = new Dictionary<string,object>();
			
			infos["nome"] = OAssociadoContribuicao.nomeAssociado;
			
			infos["descricaoPagamento"] = OAssociadoContribuicao.descricaoContribuicao;
			
            infos["valor"] = OAssociadoContribuicao.valorAtual.ToString("C");

			infos["dtVencimento"] = OAssociadoContribuicao.dtVencimentoAtual.ToShortDateString();

		    infos["linkPgto"] = urlPagamento;
			
			string tituloEmail = ONotificacao.titulo;

			return this.enviar(infos, tituloEmail);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

            var OConfig = ConfiguracaoSistemaBL.getInstance.carregar(this.AssociadoContribuicao.idOrganizacao.toInt());

            assunto = assunto.Replace("#NOME_ORGANIZACAO#", OConfig.nomeEmpresaResumo);

            // Configurar cópia oculta para emails configurados como e-mails de cobrança
		    var OConfigNotificacoes = ConfiguracaoNotificacaoBL.getInstance.carregar(this.AssociadoContribuicao.idOrganizacao.toInt());

		    var listaEmailsCopia = OConfigNotificacoes.emailCobrancaContribuicao?.Split(';').ToList();

		    if (listaEmailsCopia?.Any(x => !x.isEmpty()) == true) {

		        listaEmailsCopia.Where(x => !x.isEmpty()).ToList().ForEach(x => { this.Bcc.Add(x); });
		    }

			this.Subject = assunto;

			this.prepararMensagem();
			
			this.Body = this.Body.Replace("#NOME_ASSOCIADO#", info["nome"].ToString());

			this.Body = this.Body.Replace("#DESCRICAO_PAGAMENTO#", info["descricaoPagamento"].ToString());

            this.Body = this.Body.Replace("#VALOR#", info["valor"].ToString());

			this.Body = this.Body.Replace("#VENCIMENTO#", info["dtVencimento"].ToString());

            this.Body = this.Body.Replace("#LINK_PGTO#", info["linkPgto"].ToString());

            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);
            
			return this.disparar();
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

		    string conteudoHTML = this.ONotificacao.notificacao;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            htmlFinal = htmlFinal.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}



	}
}