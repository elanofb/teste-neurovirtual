using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Core.Events;
using WEB.Areas.Financeiro.ViewModels;
using BLL.Financeiro;
using BLL.Financeiro.Events;
using DAL.Financeiro;
using MvcFlashMessages;
using WEB.App_Infrastructure;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class DespesaPagamentoCloneController : BaseSistemaController {

        //Atributos
        private ITituloDespesaPagamentoCloneBL _TituloDespesaPagamentoCloneBL;
        private ITituloDespesaPagamentoBL _ContasAPagarPagamentoBL;
        private ITituloDespesaBL _TituloDespesaBL;

        //Propriedades
        private ITituloDespesaPagamentoCloneBL OTituloDespesaPagamentoCloneBL => _TituloDespesaPagamentoCloneBL = _TituloDespesaPagamentoCloneBL ?? new TituloDespesaPagamentoCloneBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _ContasAPagarPagamentoBL = _ContasAPagarPagamentoBL ?? new TituloDespesaPagamentoBL();
        private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();

        //Events
        private readonly EventAggregator onAtualizarValorTituloDespesa = OnAtualizarValorTituloDespesa.getInstance;

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("modal-clonar-despesa-pagamento")]
        public ActionResult modalClonarDespesaPagamento(int? id, int? idTituloDespesa) {
            
            var ViewModel = new TituloDespesaPagamentoClonarForm();

            ViewModel.TituloDespesaPagamento = this.OTituloDespesaPagamentoBL.carregar(UtilNumber.toInt32(id), null);

            if (ViewModel.TituloDespesaPagamento == null && idTituloDespesa.toInt() == 0){
                return Json(new { error = true, message = "Registro não localizado" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloDespesaPagamento == null) {
                ViewModel.TituloDespesaPagamento = new TituloDespesaPagamento();
                ViewModel.TituloDespesaPagamento.TituloDespesa = OTituloDespesaBL.carregar(idTituloDespesa.toInt());
                ViewModel.TituloDespesaPagamento.idTituloDespesa = idTituloDespesa.toInt();

                if (ViewModel.TituloDespesaPagamento.TituloDespesa?.listaTituloDespesaPagamento.Any(x => x.dtExclusao == null) == false) {
                    ViewModel.TituloDespesaPagamento.valorOriginal = ViewModel.TituloDespesaPagamento.TituloDespesa.valorTotal ?? 0;
                }
            }

            if (ViewModel.TituloDespesaPagamento.TituloDespesa == null) {
                return Json(new { error = true, message = "Registro não localizado" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloDespesaPagamento.TituloDespesa.dtExclusao.HasValue) {
                return Json(new { error = true, message = "Não é possível adicionar um pagamento a uma despesa excluída" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloDespesaPagamento.TituloDespesa.dtQuitacao.HasValue) {
                return Json(new { error = true, message = "Não é possível adicionar um pagamento a uma despesa quitada" }, JsonRequestBehavior.AllowGet);
            }

            return View(ViewModel);
        }

        //Carrega a lista de pagamento do titulo
        [HttpPost, ActionName("salvar-titulo-despesa-pagamento")]
        public ActionResult salvarTituloDespesaPagamento(TituloDespesaPagamentoClonarForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-clonar-despesa-pagamento", ViewModel);
            }

            var OTituloDespesa = this.OTituloDespesaBL.carregar(ViewModel.TituloDespesaPagamento.idTituloDespesa);

            if (OTituloDespesa == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível localizar o titulo despesa.");
                return View("modal-clonar-despesa-pagamento", ViewModel);
            }

            if (OTituloDespesa.dtQuitacao.HasValue) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível adicionar uma parcela a uma despesa quitada");
                return View("modal-clonar-despesa-pagamento", ViewModel);
            }

            ViewModel.TituloDespesaPagamento.idStatusPagamento = StatusPagamentoConst.ABERTO;
            ViewModel.TituloDespesaPagamento.dtCompetencia = ViewModel.TituloDespesaPagamento.dtCompetencia ?? ViewModel.TituloDespesaPagamento.dtVencimento;
            ViewModel.TituloDespesaPagamento.anoCompetencia = Convert.ToInt16(ViewModel.TituloDespesaPagamento.dtCompetencia?.Year);
            ViewModel.TituloDespesaPagamento.mesCompetencia = Convert.ToByte(ViewModel.TituloDespesaPagamento.dtCompetencia?.Month);

            var flagSucesso = OTituloDespesaPagamentoCloneBL.salvarClone(ViewModel.TituloDespesaPagamento);

            if (flagSucesso) {
                this.onAtualizarValorTituloDespesa.subscribe(new OnAtualizarValorTituloDespesaHandler());
                this.onAtualizarValorTituloDespesa.publish(ViewModel.TituloDespesaPagamento.idTituloDespesa as object);
                return Json(new { error = false });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível salvar o registro, tente novamente.");
            return View("modal-clonar-despesa-pagamento", ViewModel);
        }
    }
}
