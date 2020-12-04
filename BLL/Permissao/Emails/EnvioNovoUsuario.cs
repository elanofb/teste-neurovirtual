using System;
using System.Collections.Generic;
using System.Web;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Permissao;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.Permissao.Emails {

    public class EnvioNovoUsuario : EnvioEmailAdapter, IEnvioNovoUsuario {
        
        //Constantes
        private readonly ConfiguracaoSistema OConfigSistema = ConfiguracaoSistemaBL.getInstance.carregar(HttpContextFactory.Current.User.idOrganizacao());

		//Private Construtor
		private EnvioNovoUsuario(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioNovoUsuario factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioNovoUsuario(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

		//
		public UtilRetorno enviar(UsuarioSistema OUsuarioSistema, string senhaProvisoria) {

            var OConfig = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

			Dictionary<string, object> infos = new Dictionary<string,object>();

		    infos["nome"] = OUsuarioSistema.nome;
			
		    infos["login"] = OUsuarioSistema.login;

		    infos["senha"] = senhaProvisoria;

            infos["nomeOrganizacao"] = OConfigSistema.nomeEmpresaResumo;

			string tituloEmail = OConfig.novoUsuarioTitulo;

		    return this.enviar(infos, tituloEmail);
		}


        //Sobreposicao obrigatorio do metodo abstrato
	    public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

            this.Subject = assunto;

            this.Subject = this.Subject.Replace("#NOME_ORGANIZACAO#", info["nomeOrganizacao"].ToString());

            this.prepararMensagem();

            this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

            this.Body = this.Body.Replace("#NOME_ORGANIZACAO#", info["nomeOrganizacao"].ToString());

            this.Body = this.Body.Replace("#LOGIN#", info["login"].ToString());

            this.Body = this.Body.Replace("#SENHA#", info["senha"].ToString());

            this.Body = this.Body.Replace("#DOMINIO#", HttpContext.Current.Request.Url.Host);

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);

            return this.disparar();
        }

        protected override string capturarConteudoHTML(string arquivoHTML) {

		    string conteudoHTML = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao).novoUsuarioCorpo;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfigSistema.nomeEmpresaResumo);

			return htmlFinal;
		}

	}
}