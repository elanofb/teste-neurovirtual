using System;
using System.Collections.Generic;
using DAL.FaleConosco;

namespace BLL.Email {

	public class EnvioContatoPortal : EnvioEmailAdapter, IEnvioContatoPortal {

        //Atributos

		//Private Construtor
		private EnvioContatoPortal(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioContatoPortal factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioContatoPortal(idOrganizacaoParam, listaDestino, listaCopia, null);
		}


		//Customizado para essa classe especifica
		public UtilRetorno enviar(ContatoPortal OContato){

			Dictionary<string, object> infos = new Dictionary<string,object>();

			infos["nome"] = OContato.nome;

			infos["email"] = OContato.email;
			
			infos["telefone"] = !String.IsNullOrEmpty(OContato.telefone) ? OContato.telefone : "-";
			
			infos["assunto"] = OContato.assunto;
			
			infos["mensagem"] = UtilString.removeHtml(OContato.mensagem);

            return this.enviar(infos);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto = "Contato Portal") {
				
			this.Subject = String.Concat(assunto, " - ", info["assunto"]);

            this.prepararMensagem();

			this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());
			
			this.Body = this.Body.Replace("#EMAIL#", info["email"].ToString());
			
			this.Body = this.Body.Replace("#TELEFONE#", info["telefone"].ToString());
			
			this.Body = this.Body.Replace("#ASSUNTO#", info["assunto"].ToString());

			this.Body = this.Body.Replace("#MENSAGEM#", info["mensagem"].ToString());

			return this.disparar();
		}


		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

		    string conteudoHTML = "<p><strong>NOME: </strong> #NOME#</p>" +
		                          "<p><strong>E-MAIL: </strong> #EMAIL#</p>" +
		                          "<p><strong>TELEFONE: </strong> #TELEFONE#</p>" +
		                          "<p><strong>ASSUNTO: </strong> #ASSUNTO#</p>" +
		                          "<p><strong>MENSAGEM: </strong> #MENSAGEM#</p>";

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            htmlFinal = htmlFinal.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}
	}
}