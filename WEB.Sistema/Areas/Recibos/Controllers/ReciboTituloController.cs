using System;
using System.Web.Mvc;
using BLL.Configuracoes;
using BLL.ConfiguracoesRecibo;
using BLL.Financeiro;
using WEB.Areas.Recibos.ViewModels;

namespace WEB.Areas.Recibos.Controllers {

	public class ReciboTituloController : Controller {

		//Atributos
		private ITituloReceitaBL _TituloReceitaBL;
		private ITituloReceitaReciboVWBL _TituloReceitaReciboVWBL;

		//Propriedades
		private ITituloReceitaBL OTituloReceitaBL { get { return this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPadraoBL(); } }
	    private ITituloReceitaReciboVWBL OTituloReceitaReciboVWBL => this._TituloReceitaReciboVWBL = this._TituloReceitaReciboVWBL ?? new TituloReceitaReciboVWBL();

		// GET: 
		[ActionName("exibir-recibo")]
        [AllowAnonymous]
		public ActionResult exibirRecibo(string t) {

			int idTitulo = UtilNumber.toInt32(UtilCrypt.toBase64Decode(t));

			var OTituloReceita = this.OTituloReceitaBL.carregar(idTitulo);

			if (OTituloReceita == null) {
				return HttpNotFound();
			}

			if (!OTituloReceita.dtQuitacao.HasValue) {
				return HttpNotFound();
			}

            var ConfiguracaoSistema = ConfiguracaoSistemaBL.getInstance.carregar(OTituloReceita.idOrganizacao);

            var ConfiguracaoRecibo = ConfiguracaoReciboBL.getInstance.carregar(OTituloReceita.idOrganizacao);
			
			if (ConfiguracaoRecibo == null) {
				throw new Exception("Configurações de recibo não localizadas.");
			}

			var htmlRecibo = ConfiguracaoRecibo.htmlRecibo;
			
		    var OTituloRecibo = this.OTituloReceitaReciboVWBL.carregar(OTituloReceita.id);

            htmlRecibo = htmlRecibo.Replace("#LINK_LOGO#",  ConfiguracaoImagemBL.linkImagemOrganizacao(OTituloReceita.idOrganizacao, ConfiguracaoImagemBL.IMAGEM_PRINT_SISTEMA));

            htmlRecibo = htmlRecibo.Replace("#NOME_ORGANIZACAO#", ConfiguracaoSistema.nomeEmpresaResumo);

            htmlRecibo = htmlRecibo.Replace("#TEL_1_CABECALHO#", UtilString.formatPhone(OTituloRecibo.organizacaoDddTelPrincipal + " " + OTituloRecibo.organizacaoNroTelPrincipal));

            htmlRecibo = htmlRecibo.Replace("#TEL_2_CABECALHO#", UtilString.formatPhone(OTituloRecibo.organizacaoDddTelSecundario + " " + OTituloRecibo.organizacaoNroTelSecundario));

            htmlRecibo = htmlRecibo.Replace("#ENDERECO_CABECALHO#", string.Concat(OTituloRecibo.organizacaoLogradouro, ", ", OTituloRecibo.organizacaoNumero, " ", OTituloRecibo.organizacaoComplemento, ", ", OTituloRecibo.organizacaoBairro.isEmpty() ? "" : " " + OTituloRecibo.organizacaoBairro, " - ",UtilString.formatCEP(OTituloRecibo.organizacaoCep)));

            htmlRecibo = htmlRecibo.Replace("#UF_CIDADE_CABECALHO#", string.Concat(OTituloRecibo.organizacaoNomeCidade, ", ", OTituloRecibo.organizacaoSiglaEstado));

			htmlRecibo = htmlRecibo.Replace("#NUMERO#", OTituloRecibo.id.ToString().PadLeft(8, '0'));

            var valorTotal = decimal.Add(OTituloRecibo.valorTotal.toDecimal(), OTituloRecibo.valorTotalJuros.toDecimal());

            valorTotal = decimal.Subtract(valorTotal, UtilNumber.toDecimal(OTituloRecibo.valorDesconto));

		    DateTime dtQuitacao = OTituloRecibo.dtQuitacao.GetValueOrDefault();

            htmlRecibo = htmlRecibo.Replace("#VALOR#", valorTotal.ToString("C"));
			
			htmlRecibo = htmlRecibo.Replace("#NOME#", OTituloRecibo.nomeRecibo);
			
			htmlRecibo = htmlRecibo.Replace("#NRO_DOCUMENTO#", UtilString.formatCPFCNPJ(OTituloRecibo.documentoRecibo));

			htmlRecibo = htmlRecibo.Replace("#DESCRICAO#", OTituloRecibo.descricao);

			htmlRecibo = htmlRecibo.Replace("#DATA#", String.Concat(dtQuitacao.Day.ToString(), " de ", UtilDate.retornarMesPorExtenso(dtQuitacao.Month), " de ",dtQuitacao.Year.ToString()));

			htmlRecibo = htmlRecibo.Replace("#ASSINATURA#", UtilCrypt.signRecipe("tc", t) );

			var ViewModel = new ReciboVM();
			
			ViewModel.htmlRecibo = htmlRecibo;

			return View(ViewModel);

		}
	}
}