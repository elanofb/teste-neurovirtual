using System;
using System.Collections.Generic;
using BLL.Email;
using DAL.Financeiro;

namespace BLL.Financeiro.Emails {

	public class EnvioCobrancaPagamento : EnvioEmailAdapter, IEnvioCobrancaPagamento {


		//Private Construtor
		private EnvioCobrancaPagamento(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioCobrancaPagamento factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioCobrancaPagamento(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			string conteudoHTML = this.OConfiguracaoNotificacao.corpoEmailCobranca;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}

		//
		public UtilRetorno enviar(TituloReceitaPagamento OPagamento) {

			Dictionary<string, object> infos = new Dictionary<string,object>();
			
		    string urlPagamento = String.Format(UtilConfig.linkPgto, UtilString.encodeURL(UtilCrypt.toBase64Encode(OPagamento.id)));

		    infos["linkPgto"] = urlPagamento;
			
			infos["nomePessoa"] = OPagamento.TituloReceita.nomePessoa;

		    string descricaoPagamento = OPagamento.TituloReceita.descricao;

		    if (!string.IsNullOrEmpty(OPagamento.descricaoParcela)) {
		        descricaoPagamento = String.Concat(descricaoPagamento, " (", OPagamento.descricaoParcela, ")");
		    }

            infos["descricaoPagamento"] = descricaoPagamento;

			string tituloEmail = $"{OConfiguracaoSistema.tituloSistema} - {OPagamento.TituloReceita.descricao}";

			return this.enviar(infos, tituloEmail);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto ) {

			this.Subject = assunto;

			this.prepararMensagem();
			
			this.Body = this.Body.Replace("#NOME_PESSOA#", info["nomePessoa"].ToString());

			this.Body = this.Body.Replace("#DESCRICAO_PAGAMENTO#", info["descricaoPagamento"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

            this.Body = this.Body.Replace("#LINK_PGTO#", info["linkPgto"].ToString());
            
            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);

			return this.disparar();
		}

	}
	
}