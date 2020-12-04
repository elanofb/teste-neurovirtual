using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.Core.Events;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using BLL.Financeiro;
using BLL.Financeiro.Events;
using BLL.LogsAlteracoes;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using System.Collections.Generic;
using BLL.Services;
using MoreLinq;

namespace WEB.Areas.Financeiro.Controllers {
    
    public class DespesaDetalhePagamentosOperacaoController : BaseSistemaController {

        //Atributos
        private ITituloDespesaBL _TituloDespesaBL;
        private ITituloDespesaPagamentoBL _ContasAPagarPagamentoBL;
        private ILogTituloDespesaPagamentoBL _LogTituloDespesaPAgamentoBL;

        //Propriedade
        private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _ContasAPagarPagamentoBL = _ContasAPagarPagamentoBL ?? new TituloDespesaPagamentoBL();
        private ILogTituloDespesaPagamentoBL OLogTituloDespesaPagamentoBL => _LogTituloDespesaPAgamentoBL = _LogTituloDespesaPAgamentoBL ?? new LogTituloDespesaPagamentoBL();

        //Events
        private readonly EventAggregator onAtualizarValorTituloDespesa = OnAtualizarValorTituloDespesa.getInstance;

        /// <summary>
        /// Altera os campos dos pagamentos individualmente, gravando o log do mesmo
        /// </summary>
        [HttpPost, ActionName("alterar-dados-pagamentos")]
        public ActionResult alterarDadosPagamentos() {

            var id = UtilRequest.getInt32("pk");
            var nomeCampo = UtilRequest.getString("name").Trim();
            var valorCampo = UtilRequest.getString("value").Trim();
            var nomeCampoDisplay = UtilRequest.getString("nomeCampoDisplay").Trim();
            var oldValue = UtilRequest.getString("oldValue").Trim();
            var newValue = UtilRequest.getString("newValue").Trim();

            var Retorno = OLogTituloDespesaPagamentoBL.alterarCampo(id, nomeCampo, valorCampo, "", nomeCampoDisplay, oldValue, newValue);

            if (!Retorno.flagError){
                if (nomeCampo == "valorOriginal" && Retorno.flagError != true){
                    this.onAtualizarValorTituloDespesa.subscribe(new OnAtualizarValorTituloDespesaHandler());
                    this.onAtualizarValorTituloDespesa.publish(Retorno.info);
                }

                var listaOutrosPagamentos = this.OTituloDespesaPagamentoBL.listar(Retorno.info.toInt()).Select(x => new { x.dtVencimento, x.id }).ToList();

                var OPagamento = listaOutrosPagamentos.FirstOrDefault(y => y.id == id);

                var flagHabilitarAtualizarTodos = listaOutrosPagamentos.Count() > 1;
                var flagHabilitarAtualizarProximos = listaOutrosPagamentos.Any(x => x.dtVencimento > OPagamento.dtVencimento || (OPagamento.id > x.id && OPagamento.dtVencimento == x.dtVencimento));

                return Json(new {
                    Retorno.flagError,
                    Retorno.listaErros,
                    flagHabilitarAtualizarTodos,
                    flagHabilitarAtualizarProximos,
                    nomeCampoDisplay,
                    oldValue,
                    newValue
                });
            }

            return Json(new { Retorno.flagError, Retorno.listaErros });
        }

        /// <summary>
        /// Altera os campos dos pagamentos em lote de acordo com a opção selecionada
        /// Quando é realizado a alteração de um pagamento é questionado se deve alterar todos
        ///     somente aquele ou aquele e os posteriores a ele 
        /// </summary>
        [HttpPost, ActionName("alterar-dados-outros-pagamentos")]
        public ActionResult alterarDadosOutrosPagamentos() {

            var id = UtilRequest.getInt32("id");
            var tipoEdit = UtilRequest.getString("tipoEdit");
            var nomeCampo = UtilRequest.getString("campo").Trim();
            var valorCampo = UtilRequest.getString("value").Trim();
            var nomeCampoDisplay = UtilRequest.getString("nomeCampoDisplay").Trim();
            var oldValue = UtilRequest.getString("oldValue").Trim();
            var newValue = UtilRequest.getString("newValue").Trim();
            
            var OTituloDespesa = OTituloDespesaBL.listar("").Where(x => x.listaTituloDespesaPagamento.Any(y => y.id == id && y.dtExclusao == null)).Select(x => new{
                x.id,
                listaTituloDespesaPagamento = x.listaTituloDespesaPagamento.Where(y => y.dtExclusao == null).Select(y => new{ y.id, y.dtVencimento})
            }).FirstOrDefault().ToJsonObject<TituloDespesa>();

            if (OTituloDespesa == null) {
                return Json(UtilRetorno.newInstance(true, "Registro não localizado"));
            }

            var pagamentoAtual = OTituloDespesa.listaTituloDespesaPagamento.FirstOrDefault(x => x.id == id);
            var listaPagamentos = OTituloDespesa.listaTituloDespesaPagamento.Where(x => x.id != id).ToList();

            if (tipoEdit == "NEXT") {
                listaPagamentos = listaPagamentos.Where(x => x.dtVencimento > pagamentoAtual.dtVencimento || (pagamentoAtual.id > x.id && pagamentoAtual.dtVencimento == x.dtVencimento)).ToList();
            }

            if (!listaPagamentos.Any()) {
                return Json(UtilRetorno.newInstance(true, "Nenhum registro para ser alterado"));
            }

            var ORetorno = UtilRetorno.newInstance(false);

            foreach (var OPagamento in listaPagamentos) {
                var Retorno = OLogTituloDespesaPagamentoBL.alterarCampo(OPagamento.id, nomeCampo, valorCampo, "", nomeCampoDisplay, oldValue, newValue);

                if (Retorno.flagError) {
                    ORetorno.flagError = true;
                    ORetorno.listaErros.Add("Pagamento #" + OPagamento.id + " - " + Retorno.listaErros.FirstOrDefault());
                }
            }

            if (nomeCampo == "valorOriginal") {
                this.onAtualizarValorTituloDespesa.subscribe(new OnAtualizarValorTituloDespesaHandler());
                this.onAtualizarValorTituloDespesa.publish(OTituloDespesa.id as object);
            }

            return Json(ORetorno);
        }

        /// <summary>
        /// Abre a modal pra registrar o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("modal-registrar-pagamento")]
        public ActionResult modelRegistrarPagamento() {

            var ViewModel = new BaixaTituloDespesaPagamentoForm();

            List<int> ids = UtilRequest.getListInt("id");

            ViewModel.TituloDespesaPagamento = new TituloDespesaPagamento();
            ViewModel.TituloDespesaPagamento.TituloDespesa = new TituloDespesa();

            ViewModel.listaTituloDespesaPagamento = this.OTituloDespesaPagamentoBL.listar(0)
                .Where(x => ids.Contains(x.id) && x.dtPagamento == null && x.TituloDespesa.dtQuitacao == null && x.TituloDespesa.dtExclusao == null).ToList();

            if (!ViewModel.listaTituloDespesaPagamento.Any()) {
                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "Nenhum registro localizado");
                return PartialView(ViewModel);
            }

            ViewModel.TituloDespesaPagamento.TituloDespesa = ViewModel.listaTituloDespesaPagamento.DistinctBy(x => x.idTituloDespesa).Count() == 1
                ? ViewModel.listaTituloDespesaPagamento.FirstOrDefault()?.TituloDespesa : new TituloDespesa();

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Registra o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("registrar-pagamento")]
        public ActionResult registrarPagamento(BaixaTituloDespesaPagamentoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("modal-registrar-pagamento", ViewModel);
            }

            ViewModel.listaTituloDespesaPagamento.ForEach(Item => {
                Item.dtPagamento = ViewModel.TituloDespesaPagamento.dtPagamento;
                Item.idMeioPagamento = ViewModel.TituloDespesaPagamento.idMeioPagamento;
                Item.valorPago = ViewModel.TituloDespesaPagamento.valorOriginal;
                Item.valorOutrasTarifas = ViewModel.TituloDespesaPagamento.valorOutrasTarifas;
                Item.nroBanco = ViewModel.TituloDespesaPagamento.nroBanco;
                Item.nroDocumento = ViewModel.TituloDespesaPagamento.nroDocumento;
                Item.nroNotaFiscal = ViewModel.TituloDespesaPagamento.nroNotaFiscal;
                Item.nroContrato = ViewModel.TituloDespesaPagamento.nroContrato;
                Item.nroAgencia = ViewModel.TituloDespesaPagamento.nroAgencia;
                Item.nroDigitoAgencia = ViewModel.TituloDespesaPagamento.nroDigitoAgencia;
                Item.nroConta = ViewModel.TituloDespesaPagamento.nroConta;
                Item.nroDigitoConta = ViewModel.TituloDespesaPagamento.nroDigitoConta;
                Item.codigoAutorizacao = ViewModel.TituloDespesaPagamento.codigoAutorizacao;
                Item.valorJuros = ViewModel.TituloDespesaPagamento.valorJuros;
                Item.valorMulta = ViewModel.TituloDespesaPagamento.valorMulta;
            });

            var ORetorno = UtilRetorno.newInstance(false);

            foreach (var OTituloDespesaPagamento in ViewModel.listaTituloDespesaPagamento) {
                var Retorno = this.OTituloDespesaPagamentoBL.registrarPagamento(OTituloDespesaPagamento);

                if (Retorno.flagError) {
                    ORetorno.flagError = true;
                }

                ORetorno.listaErros.Add($"Pagamento #{OTituloDespesaPagamento.id} - " + Retorno.listaErros.FirstOrDefault());
            }

            if (ORetorno.flagError) {
                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, string.Join("<br/>", ORetorno.listaErros));
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, string.Join("<br/>", ORetorno.listaErros));

            return PartialView("modal-registrar-pagamento", ViewModel);
        }

        /// <summary>
        /// Cancela o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("cancelar-pagamento")]
        public ActionResult cancelarPagamento() {

            var id = UtilRequest.getInt32("id");

            var ORetorno = this.OTituloDespesaPagamentoBL.cancelarPagamento(id);

            return Json(ORetorno);
        }

        /// <summary>
        /// Faz a conciliação do pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("conciliar-pagamento")]
        public ActionResult conciliarPagamento(int id, bool flagConciliado) {

            this.OTituloDespesaPagamentoBL.conciliarPagamentos(new [] { id }, flagConciliado);

            JsonMessage Retorno = new JsonMessage() {
                error = false,
                message = String.Concat("Pagamento", (flagConciliado == false ? " não" : ""), " Conciliado")
            };

            return Json(Retorno);
        }

        [HttpGet, ActionName("modal-excluir-despesa-pagamento")]
        public ActionResult modalExcluirDespesaPagamento(int? id) {

            var ViewModel = new DespesaPagamentoExcluirForm();

            ViewModel.TituloDespesaPagamento = this.OTituloDespesaPagamentoBL.carregar(id.toInt());

            if (ViewModel.TituloDespesaPagamento == null) {
                return Json(new { flagErro = true, message = "O pagamento não pode ser localizada" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloDespesaPagamento.TituloDespesa.dtExclusao.HasValue) {
                return Json(new { flagErro = true, message = "Não é possível remover um pagamento de uma despesa excluída" }, JsonRequestBehavior.AllowGet);
            }

            var listaOutrosPagamentos = this.OTituloDespesaPagamentoBL.listar(ViewModel.TituloDespesaPagamento.idTituloDespesa)
                .Select(x => new { x.dtVencimento, x.id }).ToList();

            ViewModel.flagHabilitarAtualizarTodos = listaOutrosPagamentos.Count() > 1;
            ViewModel.flagHabilitarAtualizarProximos = listaOutrosPagamentos.Any(x => x.dtVencimento > ViewModel.TituloDespesaPagamento.dtVencimento || (x.dtVencimento == ViewModel.TituloDespesaPagamento.dtVencimento && x.id > ViewModel.TituloDespesaPagamento.id));

            return View(ViewModel);
        }


        [HttpPost, ActionName("excluir-despesa-pagamento")]
        public ActionResult excluir(DespesaPagamentoExcluirForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-excluir-despesa-pagamento", ViewModel);
            }

            var ORetorno = this.OTituloDespesaPagamentoBL.excluir(ViewModel.TituloDespesaPagamento.id, ViewModel.TituloDespesaPagamento.motivoExclusao, ViewModel.flagAtualizarOutros);

            if (!ORetorno.flagError) {

                this.onAtualizarValorTituloDespesa.subscribe(new OnAtualizarValorTituloDespesaHandler());
                this.onAtualizarValorTituloDespesa.publish(ViewModel.TituloDespesaPagamento.idTituloDespesa as object);

                return Json(ORetorno);
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, ORetorno.listaErros.FirstOrDefault());
            return View("modal-excluir-despesa-pagamento", ViewModel);
        }
    }
}
