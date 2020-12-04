using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Email;
using DAL.Notificacoes;
using Newtonsoft.Json;

namespace BLL.AssociadosOperacoes.Emails  {

	public class EnvioAtualizacaoCadastro : EnvioEmailAdapter, IEnvioAtualizacaoCadastro {

		//Atributos
		
		//Propriedades
		private NotificacaoSistema ONotificacaoSistema { get; set; }

		//Private Construtor
		private EnvioAtualizacaoCadastro(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}
		
		//Factory
		public static EnvioAtualizacaoCadastro factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta = null) { 
			return new EnvioAtualizacaoCadastro(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
		}
		

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {
			
			string htmlMaster = this.capturarMasterpage("");

			var htmlConteudo = ONotificacaoSistema.notificacao;
			
			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", htmlConteudo);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
			
		}


		//Customizado para essa classe especifica
		public UtilRetorno enviar(NotificacaoSistemaEnvio OEnvio, NotificacaoSistema ONotificacao){

			this.ONotificacaoSistema = ONotificacao;
			
            Dictionary<string, object> infos = new Dictionary<string,object>();
	
			infos["nome"] = OEnvio.nome ?? "";
				
			infos["email"] = this.To.FirstOrDefault()?.Address ?? "";
			
			infos["personalizacao"] = OEnvio.personalizacao;
				
            return this.enviar(infos, ONotificacao.titulo);
			
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {
				
			this.Subject = assunto;
			
			this.prepararMensagem("");
			
			this.Body = this.Body.Replace("#SIGLA_ASSOCIACAO#", OConfiguracaoSistema.siglaOrganizacao);

			this.Body = this.Body.Replace("#NOME_PESSOA#", info["nome"].ToString());

			string personalizacao = info["personalizacao"]?.ToString() ?? "";

			if (!personalizacao.isEmpty()){
				
				var parametros = JsonConvert.DeserializeObject<Dictionary<string, string>>(personalizacao);

				foreach (KeyValuePair<string, string> campo in parametros){
					
					this.Body = this.Body.Replace(campo.Key, campo.Value);
				}
				
			}
			
			// Configurar Anexo
			var ORetorno = UtilRetorno.newInstance(false);
			
			ORetorno = this.disparar();
			
			return ORetorno;
			
		}


	}
}