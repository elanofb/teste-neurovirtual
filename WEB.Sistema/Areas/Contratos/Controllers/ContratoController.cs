using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Contratos;
using DAL.Contratos;
using MvcFlashMessages;
using PagedList;
using WEB.App_Infrastructure;
using WEB.Areas.Contratos.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Contratos.Controllers {

    public class ContratoController : BaseSistemaController {

        //Atributos
        private IContratoBL _ContratoBL;

        //Propriedades
        private IContratoBL OContratoBL => _ContratoBL = _ContratoBL ?? new ContratoBL();

	    //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");

            var listaContrato = this.OContratoBL.listar(descricao, ativo).OrderBy(x => x.titulo);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OContratoExportacao = new ContratoExportacao();
                OContratoExportacao.baixarExcel(listaContrato.ToList());

                return null;
            }

            return View(listaContrato.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }


        //
        [HttpGet]
        public ActionResult editar(int? id) {

            ContratoForm ViewModel = new ContratoForm();

            var OContrato = this.OContratoBL.carregar(UtilNumber.toInt32(id)) ?? new Contrato();
            ViewModel.Contrato = OContrato;

            return View(ViewModel);

        }

        //
        [HttpPost]
        public ActionResult salvar(ContratoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("editar", ViewModel);
            }

            bool flagSucesso = this.OContratoBL.salvar(ViewModel.Contrato, ViewModel.OArquivoContrato);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

            return RedirectToAction("editar", new {ViewModel.Contrato.id });
        }

        //
		//[ActionName("partial-lista-pagamentos")]
		//public PartialViewResult partialListaPagamentos(int qtdeParcelas, decimal valorTotal) {
		//	var Configuracao = this.OConfiguracoesBL.financeiro();

		//	var listaPagamentosAtual = SessionContrato.getListPagamentos();

		//	var listaPagamentos = new List<TituloPagamento>(qtdeParcelas);

		//	if (qtdeParcelas == 0) {
		//		qtdeParcelas = (listaPagamentosAtual.Count == 0 ? 1 : listaPagamentosAtual.Count);
		//	}

		//	decimal valorParcela = Math.Round(Decimal.Divide(valorTotal, qtdeParcelas), 2, MidpointRounding.ToEven);
		//	DateTime dtVencimento = DateTime.Today.AddDays( Convert.ToInt32(Configuracao.boletoDiasVctoPedido) );

		//	for (int i = 1;i <= qtdeParcelas; i++) {
		//		TituloPagamento OPagamento;

		//		if (listaPagamentosAtual.Count < i) {
		//			OPagamento = new TituloPagamento { valorOriginal = Math.Round(valorParcela, 2), dtVencimento = dtVencimento };
		//		} else {
		//			OPagamento = listaPagamentosAtual[i - 1];
		//			OPagamento.valorOriginal = Math.Round(valorParcela, 2);
		//			OPagamento.dtVencimento = dtVencimento;
		//		}

		//		listaPagamentos.Add(OPagamento);
		//		dtVencimento = dtVencimento.AddMonths(1);
		//	}

		//   // SessionPedido.setListPagamentos(listaPagamentos);
		//	return PartialView(listaPagamentos);
		//}

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OContratoBL.excluir(id));
        }

    }
}