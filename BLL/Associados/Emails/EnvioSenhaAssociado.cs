using System;
using System.Collections.Generic;
using BLL.Configuracoes;
using BLL.Pessoas;
using DAL.Associados;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;

namespace BLL.Email {

	public class EnvioSenhaAssociado : EnvioEmailAdapter, IEnvioSenhaAssociado {

        //Constantes
        private readonly ConfiguracaoSistema OConfigSistema = ConfiguracaoSistemaBL.getInstance.carregar(HttpContextFactory.Current.User.idOrganizacao());

        //Private Construtor
        private EnvioSenhaAssociado(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioSenhaAssociado factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioSenhaAssociado(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

		//Customizado para essa classe especifica
		public UtilRetorno enviar(Associado OAssociado, string novaSenha) {

            var OConfig = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

            Dictionary<string, object> infos = new Dictionary<string,object>();

			infos["nomeUsuario"] = OAssociado.Pessoa.nome;
			infos["login"] = (String.IsNullOrEmpty(OAssociado.Pessoa.login)? OAssociado.Pessoa.emailPrincipal(): OAssociado.Pessoa.login);
			infos["novaSenha"] = novaSenha;

            string tituloEmail = OConfig.reenvioSenhaAssociadoTitulo;

            return this.enviar(infos, tituloEmail);
        }

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto = "Nova senha de acesso") {
				
			this.Subject = assunto;

            this.prepararMensagem();

            this.Body = this.Body.Replace("#NOME#", info["nomeUsuario"].ToString());

            this.Body = this.Body.Replace("#LOGIN#", info["login"].ToString());

            this.Body = this.Body.Replace("#SENHA#", info["novaSenha"].ToString());

			return this.disparar();
		}

        protected override string capturarConteudoHTML(string arquivoHTML) {

            string conteudoHTML = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao).reenvioSenhaAssociadoCorpo;

            string htmlMaster = this.capturarMasterpage("");

            string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            return htmlFinal;
        }
    }
}