using System;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using BLL.Configuracoes;
using BLL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Recibos.ViewModels;
using BLL.ConfiguracoesRecibo;

namespace WEB.Areas.Recibos.Controllers {

	[AllowAnonymous]
	public class ReciboContribuicaoController : Controller {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;
		private ITituloReceitaGeradorBL _TituloReceitaGeradorBL;
		private ITituloReceitaReciboVWBL _TituloReceitaReciboVWBL;

        //Propriedades
        private ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaContribuicaoBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
	    
	    private ITituloReceitaGeradorBL OTituloReceitaGeradorBL => this._TituloReceitaGeradorBL = this._TituloReceitaGeradorBL ?? new TituloReceitaGeradorContribuicaoBL();
	    private ITituloReceitaReciboVWBL OTituloReceitaReciboVWBL => this._TituloReceitaReciboVWBL = this._TituloReceitaReciboVWBL ?? new TituloReceitaReciboVWBL();

	    // GET: 
		[ActionName("exibir-recibo")]
		public ActionResult exibirRecibo(string i) {

			int idInscricao = UtilNumber.toInt32(UtilCrypt.toBase64Decode(i));

			var OAssociadoInscricao = this.OAssociadoContribuicaoBL.carregar(idInscricao);

			if (OAssociadoInscricao == null) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "O pagamento informado não foi localizado no sistema"));

				return RedirectToAction("error404", "Erro", new {area="Erros"});
			}

			var OTitulo = this.OTituloReceitaBL.carregarPorReceita(OAssociadoInscricao.id);
            
            if (OTitulo == null && OAssociadoInscricao.dtPagamento.HasValue) {

                this.OTituloReceitaGeradorBL.gerar(OAssociadoInscricao as object);

                OTitulo = this.OTituloReceitaBL.carregarPorReceita(OAssociadoInscricao.id);

                OTitulo.dtQuitacao = OAssociadoInscricao.dtPagamento;

                this.OTituloReceitaGeradorBL.salvar(OTitulo);

            }

			if (OTitulo == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "O título de pagamento informado não pôde ser localizado."));
				return RedirectToAction("error404", "Erro", new {area="Erros"});
			}

            var ConfiguracaoSistema = ConfiguracaoSistemaBL.getInstance.carregar(OAssociadoInscricao.idOrganizacao);

            string htmlRecibo = ConfiguracaoReciboBL.getInstance.carregar().htmlRecibo;

			if (htmlRecibo.isEmpty()) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não existem configurações de recibo no momento."));
				return RedirectToAction("error404", "Erro", new {area="Erros"});
			}

		    var OTituloRecibo = this.OTituloReceitaReciboVWBL.carregar(OTitulo.id);

             htmlRecibo = htmlRecibo.Replace("#LINK_LOGO#",  ConfiguracaoImagemBL.linkImagemOrganizacao(OTitulo.idOrganizacao, ConfiguracaoImagemBL.IMAGEM_PRINT_SISTEMA));

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

			htmlRecibo = htmlRecibo.Replace("#ASSINATURA#", UtilCrypt.signRecipe("ctb", i) );

			var ViewModel = new ReciboVM();
			
			ViewModel.htmlRecibo = htmlRecibo;

			return View(ViewModel);
		}
	}
}