using System;
using System.Collections.Generic;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Associados;
using DAL.Configuracoes;
using DAL.Notificacoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosOperacoes.Emails {

    public class EnvioNovaSenha : EnvioEmailAdapter, IEnvioNovaSenha {

        //Constantes
        private readonly ConfiguracaoSistema OConfigSistema = ConfiguracaoSistemaBL.getInstance.carregar(HttpContextFactory.Current.User.idOrganizacao());

        // Propriedades
        private NotificacaoSistema ONotificacao { get; set; }

		//Private Construtor
		private EnvioNovaSenha(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioNovaSenha factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioNovaSenha(idOrganizacaoParam, listaDestino, listaCopia, null);
		}


		//
		public UtilRetorno enviar(Associado OAssociado, NotificacaoSistema ONotificacao) {

		    this.ONotificacao = ONotificacao;

            var OConfig = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

			Dictionary<string, object> infos = new Dictionary<string,object>();

		    infos["nome"] = OAssociado.Pessoa.nome;
			
		    infos["login"] = OAssociado.Pessoa.login;
            
			string tituloEmail = OConfig.reenvioSenhaAssociadoTitulo.Replace("#NOME_ORGANIZACAO#", OConfiguracaoSistema.tituloSistema);

		    return this.enviar(infos, tituloEmail);
		}


        //Sobreposicao obrigatorio do metodo abstrato
	    public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

	        this.Subject = assunto;

            this.prepararMensagem();

            this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

            this.Body = this.Body.Replace("#NOME_ORGANIZACAO#", OConfigSistema.nomeEmpresaResumo);

            this.Body = this.Body.Replace("#LOGIN#", info["login"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);

            return this.disparar();
        }

        protected override string capturarConteudoHTML(string arquivoHTML) {

		    string conteudoHTML = this.ONotificacao.notificacao;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_ORGANIZACAO#", OConfigSistema.nomeEmpresaResumo);

			return htmlFinal;
		}

	}
}