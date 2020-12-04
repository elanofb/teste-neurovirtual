using System;
using System.Collections.Generic;
using System.Text;
using BLL.Configuracoes.Config;
using BLL.Email;
using DAL.Permissao;
using Textos;

namespace BLL.Permissao.Emails {

	public class EnvioRecuperacaoSenha : EnvioEmailAdapter, IEnvioRecuperacaoSenha {


		//Private Construtor
		private EnvioRecuperacaoSenha(List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioRecuperacaoSenha factory(List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioRecuperacaoSenha(listaDestino, listaCopia, null);
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			StringBuilder html = new StringBuilder();

		    html.AppendLine("Prezado(a) #NOME_USUARIO# <br /><br />");

            html.AppendLine("Você solicitou recuperação de dados de acesso no sistema #NOME_APLICACAO#.<br /><br />");

            html.AppendLine("O seu login para acesso é: <strong>#LOGIN#</strong>.<br />");

            html.AppendLine("Sua senha provisória é: <strong>#SENHA_PROVISORIA#</strong><br />");

            html.AppendLine("No seu próximo acesso, você será obrigado a informar uma nova senha.<br />");

            html.AppendLine("No caso de dúvidas entre em contato conosco.<br />");

		    string conteudoHTML = html.ToString();

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", UtilConfig.nomeAplicacaoCompleto);

			return htmlFinal;
		}

		//
		public bool enviar(UsuarioSistema OUsuarioSistema, string senhaProvisoria) {

            var OConfig = ApplicationConfigBL.getInstance.carregar();

			Dictionary<string, object> infos = new Dictionary<string,object>();

		    infos["nomeUsuario"] = OUsuarioSistema.nome;
			
			infos["nomeAplicacao"] = OConfig.siglaOrganizacao;

		    infos["login"] = OUsuarioSistema.login;

		    infos["senha"] = senhaProvisoria;

			string tituloEmail = $"{UtilConfig.nomeOrganizacao} - Recuperação de senha";

			return this.enviar(infos, tituloEmail);
		}
		

		//Sobreposicao obrigatorio do metodo abstrato
		public override bool enviar(IDictionary<string, object> info, string assunto) {

			this.Subject = assunto;

			this.prepararMensagem();
			
			this.Body = this.Body.Replace("#NOME_USUARIO#", info["nomeUsuario"].ToString());

			this.Body = this.Body.Replace("#NOME_APLICACAO#", info["nomeAplicacao"].ToString());

			this.Body = this.Body.Replace("#LOGIN#", info["login"].ToString());

			this.Body = this.Body.Replace("#SENHA_PROVISORIA#", info["senha"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);

			bool isSended = this.disparar();

			return isSended;
		}


	}
}