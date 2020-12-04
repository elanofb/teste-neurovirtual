using System;
using System.Collections.Generic;
using BLL.Email;
using BLL.Pessoas;
using DAL.AssociadosContribuicoes;
using DAL.Configuracoes;
using DAL.Notificacoes;
using BLL.Configuracoes;
using Newtonsoft.Json;

namespace BLL.Notificacoes {

	public class EnvioEmailNotificacao : EnvioEmailAdapter, IEnvioEmailNotificacao {


		//Private Construtor
		private EnvioEmailNotificacao(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioEmailNotificacao factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioEmailNotificacao(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

			string conteudoHTML = OConfiguracao.corpoEmailNovaNotificacao;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}

		//
		public UtilRetorno enviar(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio) {

			Dictionary<string, object> infos = new Dictionary<string,object>();
			
			infos["nome"] = oNotificacaoSistemaEnvio.nome;
			
			infos["nroMembro"] = oNotificacaoSistemaEnvio.nroMembro;

			infos["notificacao"] = oNotificacaoSistemaEnvio.NotificacaoSistema.notificacao;
			
			infos["personalizacao"] = oNotificacaoSistemaEnvio.personalizacao;

			string tituloEmail = String.Format("{0} - {1}", OConfiguracaoSistema.tituloSistema, oNotificacaoSistemaEnvio.NotificacaoSistema.titulo);

			return this.enviar(infos, tituloEmail);
		}
		
		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto = "Notificação") {

			this.Subject = assunto;

			this.prepararMensagem();

			string corpoNotificacao = info["notificacao"].stringOrEmpty();

			corpoNotificacao = corpoNotificacao.Replace("#NOME#", info["nome"].stringOrEmpty());

			corpoNotificacao = corpoNotificacao.Replace("#NOME_PESSOA#", info["nome"].stringOrEmpty());

			corpoNotificacao = corpoNotificacao.Replace("#NOME_ASSOCIADO#", info["nome"].stringOrEmpty());

			corpoNotificacao = corpoNotificacao.Replace("#NUMERO_CONTA#", info["nroMembro"].stringOrEmpty());

			corpoNotificacao = corpoNotificacao.Replace("#NUMERO_MEMBRO#", info["nroMembro"].stringOrEmpty());
			
			this.Body = this.Body.Replace("#NOME_PESSOA#", info["nome"].ToString());
			
			this.Body = this.Body.Replace("#NOME_ASSOCIADO#", info["nome"].ToString());

            this.Body = this.Body.Replace("#NOTIFICACAO#", corpoNotificacao);
			
			string personalizacao = info["personalizacao"]?.ToString() ?? "";

			if (!personalizacao.isEmpty()){
				
				var parametros = JsonConvert.DeserializeObject<Dictionary<string, string>>(personalizacao);

				foreach (KeyValuePair<string, string> campo in parametros){
					
					this.Body = this.Body.Replace(campo.Key, campo.Value);
				}
				
			}

			return this.disparar();
		}


	}
}