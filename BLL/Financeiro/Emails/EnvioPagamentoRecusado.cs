using System;
using System.Collections.Generic;
using BLL.Email;
using DAL.Financeiro;

namespace BLL.Financeiro.Emails {

	public class EnvioPagamentoRecusado : EnvioEmailAdapter {


		//Private Construtor
		private EnvioPagamentoRecusado(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioPagamentoRecusado factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioPagamentoRecusado(idOrganizacaoParam, listaDestino, listaCopia, null);
		}


		//
		public UtilRetorno enviar(TituloReceitaPagamento OPagamento) {

			Dictionary<string, object> infos = new Dictionary<string,object>();
			
		    string urlPagamento = UtilConfig.linkPgtoParcela(OPagamento.id);

		    infos["linkPgto"] = urlPagamento;
			
			infos["nomePessoa"] = OPagamento.TituloReceita.nomePessoa;

            infos["valor"] = OPagamento.valorTotalComDesconto().ToString("C");

            infos["dtVencimento"] = OPagamento.dtVencimento.exibirData();

		    string descricaoPagamento = OPagamento.TituloReceita.descricao;

		    if (!string.IsNullOrEmpty(OPagamento.descricaoParcela)) {

		        descricaoPagamento = String.Concat(descricaoPagamento, " (", OPagamento.descricaoParcela, ")");

		    }

            infos["descricaoPagamento"] = descricaoPagamento;

			string tituloEmail = $"{OConfiguracaoSistema.tituloSistema} - Pagamento recusado";

			return this.enviar(infos, tituloEmail);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto ) {

			this.Subject = this.OConfiguracaoNotificacao.assuntoEmailRecusaPagamento.isEmpty()? assunto : this.OConfiguracaoNotificacao.assuntoEmailRecusaPagamento;

			this.prepararMensagem();
			
			this.Body = this.Body.Replace("#NOME_PESSOA#", info["nomePessoa"].ToString());

            this.Body = this.Body.Replace("#DATA_VENCIMENTO#", info["dtVencimento"].ToString());

            this.Body = this.Body.Replace("#VALOR_COBRANCA#", info["valor"].ToString());

			this.Body = this.Body.Replace("#DESCRICAO_COBRANCA#", info["descricaoPagamento"].ToString());

            this.Body = this.Body.Replace("#LINK_PGTO#", info["linkPgto"].ToString());

			return this.disparar();
		}

        
		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			string conteudoHTML = OConfiguracaoNotificacao.corpoEmailRecusaPagamento;

		    if (conteudoHTML.isEmpty()){

		        conteudoHTML = "Caro, #NOME_PESSOA#, <br /><br /> " +
		                       "Infelizmente o pagamento de: #DESCRICAO_COBRANCA# não foi aprovado.<br />" +
		                       "<a href='#LINK_PGTO#'>Clique aqui e faça uma nova tentativa.</a>";


		    }

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}
	}
}