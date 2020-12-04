using System;
using System.Collections.Generic;
using System.Linq;
using MvcFlashMessages;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using DAL.Financeiro;
using WEB.App_Infrastructure;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers{

    [OrganizacaoFilter]
    public class DespesaCadastroController : BaseSistemaController {

        //Atributos
        private IDespesaCadastroBL _DespesaCadastroBL;

        //Propriedades
        private IDespesaCadastroBL ODespesaCadastroBL => _DespesaCadastroBL = _DespesaCadastroBL ?? new DespesaCadastroBL();

        [HttpGet]
        public ActionResult index(){

            var ViewModel = new DespesaCadastroForm();

            ViewModel.TituloDespesa = new TituloDespesa();

            ViewModel.urlRetorno = UtilRequest.getString("urlRetorno");

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult index(DespesaCadastroForm ViewModel) {
            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            ViewModel.carregarIdPessoa();
            ViewModel.gerarPagamento();

            ViewModel.TituloDespesa.dtVencimento = ViewModel.TituloDespesa.listaTituloDespesaPagamento.OrderBy(x => x.dtVencimento).FirstOrDefault()?.dtVencimento;

            var anoCompetencia = ViewModel.TituloDespesa.dtDespesa.HasValue ? ViewModel.TituloDespesa.dtDespesa?.Year : ViewModel.TituloDespesa.dtVencimento?.Year;
            var mesCompetencia = ViewModel.TituloDespesa.dtDespesa.HasValue ? ViewModel.TituloDespesa.dtDespesa?.Month : ViewModel.TituloDespesa.dtVencimento?.Month;

            ViewModel.TituloDespesa.anoCompetencia = Convert.ToInt16(anoCompetencia);
            ViewModel.TituloDespesa.mesCompetencia = Convert.ToByte(mesCompetencia);

            ViewModel.tratarPagamentos();

            var flagSucesso = ODespesaCadastroBL.salvar(ViewModel.TituloDespesa);

            if (flagSucesso) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");
                
                return RedirectToAction("editar", "DespesaDetalhe", new { area = "Financeiro", ViewModel.TituloDespesa.id, ViewModel.urlRetorno });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");
            return View(ViewModel);
        }

        [ActionName("partial-gerar-despesas-pagamento-form")]
        public ActionResult gerarDespesasPagamentosForm() {

            var valorTotal = UtilNumber.toDecimal(UtilRequest.getString("valorTotal"));
            var dtPrimeiroVencimento = UtilRequest.getDateTime("dtPrimeiroVencimento") ?? DateTime.Now.AddMonths(1);
            var parcelas = UtilRequest.getInt32("parcelas");
            var flagValorTotalParcelamento = UtilRequest.getString("flagValorTotalParcelamento");
            var valorParcelas = UtilNumber.toDecimal(UtilRequest.getString("valorParcelas"));
            var valorMulta = UtilNumber.toDecimal(UtilRequest.getString("valorMulta"));
            var valorJuros = UtilNumber.toDecimal(UtilRequest.getString("valorJuros"));
            var valorDesconto = UtilNumber.toDecimal(UtilRequest.getString("valorDesconto"));
            var flagCompleteDtCompetencia = UtilRequest.getString("flagCompleteDtCompetencia");
            var dtDespesa = UtilRequest.getDateTime("dtDespesa");

            if (flagValorTotalParcelamento == "S" && !(valorTotal > 0)) {
                return Json(new { error = true, message = "Informe o valor total da despesa" });
            }

            if (!(valorParcelas > 0) && flagValorTotalParcelamento != "S") {
                return Json(new {error = true, message = "Informe o valor de cada parcela" });
            }

            if (parcelas < 2) {
                return Json(new { error = true, message = "Informe o numero de parcelas a serem geradas" });
            }

            var dtVencimento = dtPrimeiroVencimento;
            var listaTituloDespesaPagamento = new List<TituloDespesaPagamento>();

            for (var x = 0; x < parcelas; x++) {

                var OTituloDespesaPagamento = new TituloDespesaPagamento();
                OTituloDespesaPagamento.dtVencimento = dtVencimento.Date;

                OTituloDespesaPagamento.valorOriginal = flagValorTotalParcelamento == "S" ? Math.Round((UtilNumber.toDecimal(valorTotal) / UtilNumber.toInt32(parcelas)), 2) : valorParcelas;
                OTituloDespesaPagamento.valorMulta = valorMulta;
                OTituloDespesaPagamento.valorJuros = valorJuros;
                OTituloDespesaPagamento.valorDesconto = valorDesconto;

                OTituloDespesaPagamento.descParcela = (x+1) + "° Parcela";

                dtVencimento = dtVencimento.AddMonths(1);

                var daysInMonth = DateTime.DaysInMonth(dtVencimento.Year, dtVencimento.Month);
                if (dtVencimento.Day < dtPrimeiroVencimento.Day && daysInMonth > dtVencimento.Day) {
                    var days = daysInMonth > dtPrimeiroVencimento.Day ? dtPrimeiroVencimento.Day : daysInMonth;
                    dtVencimento = new DateTime(dtVencimento.Year, dtVencimento.Month, days);
                }

                OTituloDespesaPagamento.dtCompetencia = (flagCompleteDtCompetencia == "S" ? dtDespesa : flagCompleteDtCompetencia == "N" ? OTituloDespesaPagamento.dtVencimento : (DateTime?)null);

                listaTituloDespesaPagamento.Add(OTituloDespesaPagamento);
            }

            //Faz um correção de valores quando for fornecido o valor total do parcelamento
            if (flagValorTotalParcelamento == "S") {
                var valorTotalPagamentos = 0M;
                listaTituloDespesaPagamento.ForEach(item => {
                    valorTotalPagamentos += item.valorOriginal;
                });

                listaTituloDespesaPagamento.LastOrDefault().valorOriginal += UtilNumber.toDecimal(valorTotal - valorTotalPagamentos);
            }

            var ViewModel = new DespesaCadastroForm();
            ViewModel.TituloDespesa = new TituloDespesa();
            ViewModel.TituloDespesa.listaTituloDespesaPagamento = listaTituloDespesaPagamento;

            return View(ViewModel);
        }

        [HttpPost, ActionName("partial-dados-pagamento")]
        public PartialViewResult partialDadosPagamento(DespesaCadastroForm ViewModel) {
            
            return PartialView(ViewModel);
        }

        [HttpPost, ActionName("partial-dados-pagamento-parcelado")]
        public PartialViewResult partialDadosPagamentoParcelado(DespesaCadastroForm ViewModel) {

            return PartialView(ViewModel);
        }
    }
}