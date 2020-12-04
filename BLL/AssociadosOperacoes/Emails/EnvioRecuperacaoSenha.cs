using System;
using System.Collections.Generic;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes.Emails  {

	public class EnvioRecuperacaoSenha : EnvioEmailAdapter, IEnvioRecuperacaoSenha {

		//Atributos

		//Propriedades

		//Private Construtor
		private EnvioRecuperacaoSenha(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioRecuperacaoSenha factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta = null) { 
			return new EnvioRecuperacaoSenha(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
		}


		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {
            
            var conteudoHTML = this.OConfiguracaoNotificacao.corpoEmailRecuperacaoSenhaAssociado;
            
            string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}


		//Customizado para essa classe especifica
		public UtilRetorno enviar(NotificacaoSistemaEnvio OEnvio, string linkRecuperacao = "") {

            Dictionary<string, object> infos = new Dictionary<string,object>();

			infos["id"] = OEnvio.idReferencia.ToString();

			infos["nome"] = OEnvio.nome;

		    infos["a"] = UtilCrypt.toBase64Encode(OEnvio.idReferencia.toInt());

		    infos["acr"] = UtilCrypt.SHA512(OEnvio.idReferencia.toInt().ToString());
            
		    infos["mlcr"] = UtilCrypt.SHA512(OEnvio.email);

            var parametros = $"?a={ infos["a"] }&acr={ infos["acr"] }&mlcr={ infos["mlcr"]  }";

            if (!linkRecuperacao.isEmpty()) {
                linkRecuperacao = String.Concat(linkRecuperacao, parametros);
            }

		    if (linkRecuperacao.isEmpty()) {
                linkRecuperacao = ConfiguracaoLinkBaseBL.linkAreaAssociado(idOrganizacao, $"minhaconta/alteracaosenha/nova-senha{ parametros }");
            }
            
            infos["link"] = linkRecuperacao;

		    string tituloEmail = this.OConfiguracaoNotificacao.tituloEmailRecuperacaoSenhaAssociado.Replace("#NOME_ORGANIZACAO#", OConfiguracaoSistema.tituloSistema);

            return this.enviar(infos, tituloEmail);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {
				
			this.Subject = assunto;

			this.prepararMensagem("");
            
			this.Body = this.Body.Replace("#SIGLA_ASSOCIACAO#", OConfiguracaoSistema.siglaOrganizacao);

            // Para corpo de email padrão
			this.Body = this.Body.Replace("#LINK_RECUPERACAO#", info["link"].ToString());

            // Para corpo de email de organizações com portal
		    this.Body = this.Body.Replace("#PARAMETRO_A#", info["a"].ToString());

		    this.Body = this.Body.Replace("#PARAMETRO_ACR#", info["acr"].ToString());

		    this.Body = this.Body.Replace("#PARAMETRO_MLCR#", info["mlcr"].ToString());
            // Fim 

			this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Today.ToLongDateString());

			return this.disparar();
		}
	}
}