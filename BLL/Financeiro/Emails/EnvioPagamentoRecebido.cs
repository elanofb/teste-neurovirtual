using System;
using System.Collections.Generic;
using BLL.Email;
using DAL.Financeiro;

namespace BLL.Financeiro.Emails {

	public class EnvioPagamentoRecebido : EnvioEmailAdapter {


		//Private Construtor
		private EnvioPagamentoRecebido(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioPagamentoRecebido factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioPagamentoRecebido(idOrganizacaoParam, listaDestino, listaCopia, null);
		}


		//
		public UtilRetorno enviar(TituloReceitaPagamento OPagamento) {

			Dictionary<string, object> infos = new Dictionary<string,object>();
			
		    string urlPagamento = UtilConfig.linkPgtoParcela(OPagamento.id);

		    infos["linkPgto"] = urlPagamento;
			
			infos["nomePessoa"] = OPagamento.TituloReceita.nomePessoa;

            infos["valor"] = OPagamento.valorTotalComDesconto().ToString("C");

            infos["valorRecebido"] = OPagamento.valorRecebido.GetValueOrDefault().ToString("C");

            infos["dtVencimento"] = OPagamento.dtVencimento.exibirData();

            infos["dtPagamento"] = OPagamento.dtPagamento.exibirData();

		    string descricaoPagamento = OPagamento.TituloReceita.descricao;

		    if (!string.IsNullOrEmpty(OPagamento.descricaoParcela)) {

		        descricaoPagamento = String.Concat(descricaoPagamento, " (", OPagamento.descricaoParcela, ")");

		    }

            infos["descricaoPagamento"] = descricaoPagamento;

			string tituloEmail = $"{OConfiguracaoSistema.tituloSistema} - Pagamento recebido";

			return this.enviar(infos, tituloEmail);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto ) {

			this.Subject = this.OConfiguracaoNotificacao.tituloEmailPagamentoRecebido.isEmpty()? assunto : this.OConfiguracaoNotificacao.tituloEmailPagamentoRecebido;

			this.prepararMensagem();
			
			this.Body = this.Body.Replace("#NOME_PESSOA#", info["nomePessoa"].ToString());

            this.Body = this.Body.Replace("#DATA_VENCIMENTO#", info["dtVencimento"].ToString());

            this.Body = this.Body.Replace("#VALOR_COBRANCA#", info["valor"].ToString());

            this.Body = this.Body.Replace("#VALOR_RECEBIDO#", info["valorRecebido"].ToString());

            this.Body = this.Body.Replace("#DATA_PAGAMENTO#", info["dtPagamento"].ToString());

			this.Body = this.Body.Replace("#DESCRICAO_COBRANCA#", info["descricaoPagamento"].ToString());

            this.Body = this.Body.Replace("#LINK_PGTO#", info["linkPgto"].ToString());

			return this.disparar();
		}

        
		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			string conteudoHTML = this.OConfiguracaoNotificacao.corpoEmailPagamentoRecebido;

		    if (conteudoHTML.isEmpty()){

		        conteudoHTML = "Caro, #NOME_PESSOA#, <br /><br /> " +
		                       "Confirmamos a aprovação do pagamento realizado em #DATA_PAGAMENTO# no valor de #VALOR_RECEBIDO#.<br />"+
                               "#DESCRICAO_COBRANCA# <br /><br />";


		    }

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}
	}
}