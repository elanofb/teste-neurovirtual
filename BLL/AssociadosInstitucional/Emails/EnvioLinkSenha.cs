using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Configuracoes;
using BLL.Email;
using BLL.Pessoas;
using DAL.Associados;
using DAL.Configuracoes;
using DAL.Configuracoes.Default;
using DAL.Pessoas;
using Textos;

namespace BLL.AssociadosInstitucional.Emails  {

	public class EnvioLinkSenha : EnvioEmailAdapter, IEnvioLinkSenha {

		//Atributos

		//Propriedades

		//Private Construtor
		private EnvioLinkSenha(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioLinkSenha factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta = null) { 
			return new EnvioLinkSenha(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
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
		public UtilRetorno enviar(Associado OAssociado, string linkRecuperacao = "") {

            Dictionary<string, object> infos = new Dictionary<string,object>();

			infos["id"] = OAssociado.id.ToString();

			infos["nome"] = OAssociado.Pessoa.nome;

		    infos["a"] = UtilCrypt.toBase64Encode(OAssociado.id);

		    infos["acr"] = UtilCrypt.SHA512(OAssociado.id.ToString());

		    var email = OAssociado.Pessoa.ToEmailList().FirstOrDefault();

		    infos["mlcr"] = UtilCrypt.SHA512(email);

			var parametros = $"?a={ infos["a"] }&acr={ infos["acr"] }&mlcr={ infos["mlcr"]  }";

            if (!linkRecuperacao.isEmpty()) {
                linkRecuperacao = String.Concat(linkRecuperacao, parametros);
            }

		    if (linkRecuperacao.isEmpty()) {
                linkRecuperacao = ConfiguracaoLinkBaseBL.linkAreaAssociado(idOrganizacao, $"minhaconta/alteracaosenha/nova-senha{ parametros }");
            }
            
            infos["link"] = linkRecuperacao;


            return this.enviar(infos, $"Recuperação de senha - { OConfiguracaoSistema.tituloSistema }");
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