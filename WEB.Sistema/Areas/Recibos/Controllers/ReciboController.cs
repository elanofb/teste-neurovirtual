using System;
using System.Web.Mvc;
using BLL.Configuracoes;
using BLL.ConfiguracoesRecibo;
using BLL.Financeiro;
using DAL.Financeiro;
using WEB.Areas.Recibos.ViewModels;

namespace WEB.Areas.Recibos.Controllers {
	public class ReciboController : Controller {

		//Atributos
		private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;
		
		private ITituloReceitaReciboVWBL _TituloReceitaReciboVWBL;

		//Propriedades
		private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL { get { return this._TituloReceitaPagamentoBL = this._TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL(); } }
		
		private ITituloReceitaReciboVWBL OTituloReceitaReciboVWBL => this._TituloReceitaReciboVWBL = this._TituloReceitaReciboVWBL ?? new TituloReceitaReciboVWBL();

		// GET: 
		[ActionName("exibir-recibo")]
		public ActionResult exibirRecibo(string r) {

			int idTituloPagamento = UtilNumber.toInt32(UtilCrypt.toBase64Decode(r));

			var OPagamentoRecibo = this.OTituloReceitaPagamentoBL.carregar(idTituloPagamento);

			var OTituloRecibo = this.OTituloReceitaReciboVWBL.carregar(OPagamentoRecibo.idTituloReceita);
			
			if (OPagamentoRecibo == null || OTituloRecibo == null) {
				return HttpNotFound();
			}

			if (!OPagamentoRecibo.dtPagamento.HasValue || !OPagamentoRecibo.valorRecebido.HasValue) {
				return HttpNotFound();
			}

			var ConfiguracaoSistema = ConfiguracaoSistemaBL.getInstance.carregar(OPagamentoRecibo.idOrganizacao);
			
			string htmlRecibo = ConfiguracaoReciboBL.getInstance.carregar().htmlRecibo;

			if (String.IsNullOrEmpty(htmlRecibo)) {
				throw new Exception("Configurações de recibo não localizadas.");
			}

            htmlRecibo = htmlRecibo.Replace("#LINK_LOGO#", ConfiguracaoImagemBL.linkImagemOrganizacao(OPagamentoRecibo.idOrganizacao, ConfiguracaoImagemBL.IMAGEM_PRINT_SISTEMA));

			htmlRecibo = htmlRecibo.Replace("#NOME_ORGANIZACAO#", ConfiguracaoSistema.nomeEmpresaResumo);

			htmlRecibo = htmlRecibo.Replace("#TEL_1_CABECALHO#", UtilString.formatPhone(OTituloRecibo.organizacaoDddTelPrincipal + " " + OTituloRecibo.organizacaoNroTelPrincipal));

			htmlRecibo = htmlRecibo.Replace("#TEL_2_CABECALHO#", UtilString.formatPhone(OTituloRecibo.organizacaoDddTelSecundario + " " + OTituloRecibo.organizacaoNroTelSecundario));

			htmlRecibo = htmlRecibo.Replace("#ENDERECO_CABECALHO#", string.Concat(OTituloRecibo.organizacaoLogradouro, ", ", OTituloRecibo.organizacaoNumero, " ", OTituloRecibo.organizacaoComplemento, ", ", OTituloRecibo.organizacaoBairro.isEmpty() ? "" : " " + OTituloRecibo.organizacaoBairro, " - ",UtilString.formatCEP(OTituloRecibo.organizacaoCep)));

			htmlRecibo = htmlRecibo.Replace("#UF_CIDADE_CABECALHO#", string.Concat(OTituloRecibo.organizacaoNomeCidade, ", ", OTituloRecibo.organizacaoSiglaEstado));
			
			htmlRecibo = htmlRecibo.Replace("#NUMERO#", OPagamentoRecibo.id.ToString().PadLeft(8, '0'));
			
			htmlRecibo = htmlRecibo.Replace("#VALOR#", OPagamentoRecibo.valorOriginal.ToString("C"));
			
			htmlRecibo = htmlRecibo.Replace("#NOME#", OTituloRecibo.nomeRecibo);

			htmlRecibo = htmlRecibo.Replace("#DESCRICAO#", OPagamentoRecibo.descricaoPagamento());

			htmlRecibo = htmlRecibo.Replace("#DATA#", String.Concat(DateTime.Now.Day.ToString(), " de ", UtilDate.retornarMesPorExtenso(DateTime.Now.Month), " de ",DateTime.Now.Year.ToString()));

			htmlRecibo = htmlRecibo.Replace("#ASSINATURA#", UtilCrypt.signRecipe("tp", r) );
			
			htmlRecibo = htmlRecibo.Replace("#NRO_DOCUMENTO#", UtilString.formatCPFCNPJ(OTituloRecibo.documentoRecibo));

			var ViewModel = new ReciboVM();
			
			ViewModel.htmlRecibo = htmlRecibo;

			return View(ViewModel);

		}
	}
}