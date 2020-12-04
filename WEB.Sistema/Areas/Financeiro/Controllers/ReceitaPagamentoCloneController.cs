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
    public class ReceitaPagamentoCloneController : BaseSistemaController {

        //Atributos
        private ITituloReceitaPagamentoCloneBL _TituloReceitaPagamentoCloneBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;
        private TituloReceitaPadraoBL _TituloReceitaBL;

        //Propriedades
        private ITituloReceitaPagamentoCloneBL OTituloReceitaPagamentoCloneBL => _TituloReceitaPagamentoCloneBL = _TituloReceitaPagamentoCloneBL ?? new TituloReceitaPagamentoCloneBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();
        private TituloReceitaPadraoBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();

        //Events
        private readonly EventAggregator onAtualizarValorTituloReceita = OnAtualizarValorTituloReceita.getInstance;

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("modal-clonar-receita-pagamento")]
        public ActionResult modalClonarReceitaPagamento(int? id, int? idTituloReceita) {
            
            var ViewModel = new TituloReceitaPagamentoClonarForm();

            ViewModel.TituloReceitaPagamento = this.OTituloReceitaPagamentoBL.carregar(UtilNumber.toInt32(id), null);

            if (ViewModel.TituloReceitaPagamento == null && idTituloReceita.toInt() == 0){
                return Json(new { error = true, message = "Registro não localizado" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloReceitaPagamento == null) {
                ViewModel.TituloReceitaPagamento = new TituloReceitaPagamento();
                ViewModel.TituloReceitaPagamento.TituloReceita = OTituloReceitaBL.carregar(idTituloReceita.toInt());
                ViewModel.TituloReceitaPagamento.idTituloReceita = idTituloReceita.toInt();

                if (ViewModel.TituloReceitaPagamento.TituloReceita?.listaTituloReceitaPagamento.Any(x => x.dtExclusao == null) == false) {
                    ViewModel.TituloReceitaPagamento.valorOriginal = ViewModel.TituloReceitaPagamento.TituloReceita.valorTotal ?? 0;
                }
            }

            if (ViewModel.TituloReceitaPagamento.TituloReceita == null) {
                return Json(new { error = true, message = "Registro não localizado"}, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloReceitaPagamento.TituloReceita.dtExclusao.HasValue) {
                return Json(new { error = true, message = "Não é possível adicionar um pagamento a uma receita excluída" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloReceitaPagamento.TituloReceita.dtQuitacao.HasValue) {
                return Json(new { error = true, message = "Não é possível adicionar um pagamento a uma receita quitada" }, JsonRequestBehavior.AllowGet);
            }

            return View(ViewModel);
        }

        //Carrega a lista de pagamento do titulo
        [HttpPost, ActionName("salvar-titulo-receita-pagamento")]
        public ActionResult salvarTituloReceitaPagamento(TituloReceitaPagamentoClonarForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-clonar-receita-pagamento", ViewModel);
            }

            var OTituloReceita = this.OTituloReceitaBL.carregar(ViewModel.TituloReceitaPagamento.idTituloReceita);

            if (OTituloReceita == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível localizar o titulo despesa.");
                return View("modal-clonar-receita-pagamento", ViewModel);
            }

            if (OTituloReceita.dtQuitacao.HasValue) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível adicionar uma parcela a uma despesa quitada");
                return View("modal-clonar-receita-pagamento", ViewModel);
            }

            ViewModel.TituloReceitaPagamento.idStatusPagamento = StatusPagamentoConst.ABERTO;
            ViewModel.TituloReceitaPagamento.dtCompetencia = ViewModel.TituloReceitaPagamento.dtCompetencia ?? ViewModel.TituloReceitaPagamento.dtVencimento;
            ViewModel.TituloReceitaPagamento.anoCompetencia = Convert.ToInt16(ViewModel.TituloReceitaPagamento.dtCompetencia?.Year);
            ViewModel.TituloReceitaPagamento.mesCompetencia = Convert.ToByte(ViewModel.TituloReceitaPagamento.dtCompetencia?.Month);

            var flagSucesso = OTituloReceitaPagamentoCloneBL.salvarClone(ViewModel.TituloReceitaPagamento);

            if (flagSucesso) {
                this.onAtualizarValorTituloReceita.subscribe(new OnAtualizarValorTituloReceitaHandler());
                this.onAtualizarValorTituloReceita.publish(ViewModel.TituloReceitaPagamento.idTituloReceita as object);
                return Json(new { error = false });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível salvar o registro, tente novamente.");
            return View("modal-clonar-receita-pagamento", ViewModel);
        }
    }
}
