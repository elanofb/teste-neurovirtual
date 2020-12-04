using System;
using System.Collections.Generic;
using BLL.Email;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes.Emails {

	public class EnvioPagamentoContribuicao : EnvioEmailAdapter, IEnvioPagamentoContribuicao {

		//Atributos

        //Propriedades
	    private AssociadoContribuicao AssociadoContribuicao;

		//Private Construtor
		private EnvioPagamentoContribuicao(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioPagamentoContribuicao factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta = null) { 
			return new EnvioPagamentoContribuicao(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
		}

        //
		public UtilRetorno enviar(AssociadoContribuicao OAssociadoContribuicao) {

		    this.AssociadoContribuicao = OAssociadoContribuicao;

			Dictionary<string, object> infos = new Dictionary<string,object>();
			
			infos["nome"] = OAssociadoContribuicao.Associado.Pessoa.nome;
			
			infos["descricaoPagamento"] = OAssociadoContribuicao.Contribuicao.descricao;
			
			infos["dtVencimento"] = OAssociadoContribuicao.dtVencimentoAtual.ToShortDateString();
			
			infos["dtPagamento"] = OAssociadoContribuicao.dtPagamento.HasValue? OAssociadoContribuicao.dtPagamento.Value.ToShortDateString(): "";

		    infos["valor"] = OAssociadoContribuicao.valorAtual.ToString("C");
			
			string tituloEmail = OAssociadoContribuicao.Contribuicao.emailPagamentoTitulo;

			return this.enviar(infos, tituloEmail);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {
				
			this.Subject = assunto;

			this.prepararMensagem();

			this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

			this.Body = this.Body.Replace("#DESCRICAO_PAGAMENTO#", info["descricaoPagamento"].ToString());

			this.Body = this.Body.Replace("#VENCIMENTO#", info["dtVencimento"].ToString());

			this.Body = this.Body.Replace("#DATA_PAGAMENTO#", info["dtPagamento"].ToString());

            this.Body = this.Body.Replace("#DADOS_PAGAMENTO#", "");

            this.Body = this.Body.Replace("#VALOR#", info["valor"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

            this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);

			return this.disparar();
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

		    string conteudoHTML = this.AssociadoContribuicao.Contribuicao.emailPagamentoHtml;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}



	}
}