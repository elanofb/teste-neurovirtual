using System;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.Pedidos;
using DAL.Financeiro;
using WEB.Areas.Recibos.ViewModels;
using BLL.ConfiguracoesRecibo;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Recibos.Controllers {

	[AllowAnonymous]
	public class ReciboPedidoController : Controller {

		//Atributos
		private IPedidoBL _PedidoBL;
		private ITituloReceitaBL _TituloReceitaBL;

		//Propriedades
		private IPedidoBL OPedidoBL { get { return this._PedidoBL = this._PedidoBL ?? new PedidoBL(); } }
		private ITituloReceitaBL OTituloReceitaBL { get { return this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPedidoBL(); } }

		// GET: 
		[ActionName("exibir-recibo")]
		public ActionResult exibirRecibo(string i) {

			int idPedido = UtilNumber.toInt32(UtilCrypt.toBase64Decode(i));

			var OPedido = this.OPedidoBL.carregar(idPedido);

			if (OPedido == null) {
				return HttpNotFound();
			}

			var OTitulo = this.OTituloReceitaBL.carregarPorReceita(OPedido.id);

			if (OTitulo == null || !OTitulo.dtQuitacao.HasValue) {
				return HttpNotFound();
			}

			string htmlRecibo = ConfiguracaoReciboBL.getInstance.carregar().htmlRecibo;

			if (String.IsNullOrEmpty(htmlRecibo)) {
				throw new Exception("Configuracoes de recibo não localizadas.");
			}

            var linkLogo = String.Concat(UtilConfig.linkAbsSistema, "/upload/logotipo/", HttpContextFactory.Current.User.idOrganizacao(), "/logotipo_print.png");

            htmlRecibo = htmlRecibo.Replace("#LINK_LOGO#", linkLogo);

			htmlRecibo = htmlRecibo.Replace("#NUMERO_RECIBO#", OTitulo.id.ToString().PadLeft(8, '0'));
			
			htmlRecibo = htmlRecibo.Replace("#VALOR_RECIBO#", OTitulo.valorLiquido().ToString("C"));
			
			htmlRecibo = htmlRecibo.Replace("#NOME_RECIBO#", OTitulo.nomeRecibo);
			
			htmlRecibo = htmlRecibo.Replace("#NRO_DOCUMENTO#", UtilString.formatCPFCNPJ(OTitulo.documentoRecibo));

			htmlRecibo = htmlRecibo.Replace("#DESCRICAO_RECIBO#", OTitulo.descricao);

			htmlRecibo = htmlRecibo.Replace("#DATA_RECIBO#", String.Concat(DateTime.Now.Day.ToString(), " de ", UtilDate.retornarMesPorExtenso(DateTime.Now.Month), " de ",DateTime.Now.Year.ToString()));

			htmlRecibo = htmlRecibo.Replace("#ASSINATURA_RECIBO#", UtilCrypt.signRecipe("an", i) );

			var ViewModel = new ReciboVM();
			
			ViewModel.htmlRecibo = htmlRecibo;

			return View(ViewModel);
		}
	}
}