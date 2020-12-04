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
    public class ReceitaCadastroController : BaseSistemaController {

        //Atributos
        private IReceitaCadastroBL _ReceitaCadastroBL;

        //Propriedades
        private IReceitaCadastroBL OReceitaCadastroBL => _ReceitaCadastroBL = _ReceitaCadastroBL ?? new ReceitaCadastroBL();

        [HttpGet]
        public ActionResult index(){

            var ViewModel = new ReceitaCadastroForm();

            ViewModel.TituloReceita = new TituloReceita();

            ViewModel.urlRetorno = UtilRequest.getString("urlRetorno");

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult index(ReceitaCadastroForm ViewModel) {
            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            ViewModel.carregarIdPessoa();
            ViewModel.gerarPagamento();

            ViewModel.TituloReceita.dtVencimento = ViewModel.TituloReceita.listaTituloReceitaPagamento.OrderBy(x => x.dtVencimento).FirstOrDefault()?.dtVencimento;
            ViewModel.TituloReceita.idTipoReceita = Convert.ToByte(TipoReceitaConst.OUTROS);

            var anoCompetencia = ViewModel.TituloReceita.dtCompetencia.HasValue ? ViewModel.TituloReceita.dtCompetencia?.Year : ViewModel.TituloReceita.dtVencimento?.Year;
            var mesCompetencia = ViewModel.TituloReceita.dtCompetencia.HasValue ? ViewModel.TituloReceita.dtCompetencia?.Month : ViewModel.TituloReceita.dtVencimento?.Month;

            ViewModel.TituloReceita.anoCompetencia = Convert.ToInt16(anoCompetencia);
            ViewModel.TituloReceita.mesCompetencia = Convert.ToByte(mesCompetencia);

            ViewModel.tratarPagamentos();

            var flagSucesso = OReceitaCadastroBL.salvar(ViewModel.TituloReceita);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");
                return RedirectToAction("editar", "ReceitaDetalhe", new { area = "Financeiro", ViewModel.TituloReceita.id, ViewModel.urlRetorno });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");
            return View(ViewModel);
        }

        [ActionName("partial-gerar-receitas-pagamento-form")]
        public ActionResult gerarReceitasPagamentosForm() {

            var valorTotal = UtilNumber.toDecimal(UtilRequest.getString("valorTotal"));
            var dtPrimeiroVencimento = UtilRequest.getDateTime("dtPrimeiroVencimento") ?? DateTime.Now.AddMonths(1);
            var parcelas = UtilRequest.getInt32("parcelas");
            var flagValorTotalParcelamento = UtilRequest.getString("flagValorTotalParcelamento");
            var valorParcelas = UtilNumber.toDecimal(UtilRequest.getString("valorParcelas"));
            var flagCompleteDtCompetencia = UtilRequest.getString("flagCompleteDtCompetencia");
            var dtCompetencia = UtilRequest.getDateTime("dtCompetencia");

            if (flagValorTotalParcelamento == "S" && !(valorTotal > 0)) {
                return Json(new { error = true, message = "Informe o valor total da receita" });
            }

            if (!(valorParcelas > 0) && flagValorTotalParcelamento != "S") {
                return Json(new {error = true, message = "Informe o valor de cada parcela" });
            }

            if (parcelas < 2) {
                return Json(new { error = true, message = "Informe o numero de parcelas a serem geradas" });
            }

            var dtVencimento = dtPrimeiroVencimento;
            var listaTituloReceitaPagamento = new List<TituloReceitaPagamento>();

            for (var x = 0; x < parcelas; x++) {

                var OTituloReceitaPagamento = new TituloReceitaPagamento();
                OTituloReceitaPagamento.dtVencimento = dtVencimento.Date;

                OTituloReceitaPagamento.valorOriginal = flagValorTotalParcelamento == "S" ? Math.Round((UtilNumber.toDecimal(valorTotal) / UtilNumber.toInt32(parcelas)), 2) : valorParcelas;

                OTituloReceitaPagamento.descricaoParcela = (x+1) + "° Parcela";

                dtVencimento = dtVencimento.AddMonths(1);

                var daysInMonth = DateTime.DaysInMonth(dtVencimento.Year, dtVencimento.Month);
                if (dtVencimento.Day < dtPrimeiroVencimento.Day && daysInMonth > dtVencimento.Day) {
                    var days = daysInMonth > dtPrimeiroVencimento.Day ? dtPrimeiroVencimento.Day : daysInMonth;
                    dtVencimento = new DateTime(dtVencimento.Year, dtVencimento.Month, days);
                }

                OTituloReceitaPagamento.dtPrevisaoPagamento = OTituloReceitaPagamento.dtVencimento;

                OTituloReceitaPagamento.dtCompetencia = (flagCompleteDtCompetencia == "S" ? dtCompetencia : flagCompleteDtCompetencia == "N" ? OTituloReceitaPagamento.dtVencimento : (DateTime?)null);

                listaTituloReceitaPagamento.Add(OTituloReceitaPagamento);
            }

            //Faz um correção de valores quando for fornecido o valor total do parcelamento
            if (flagValorTotalParcelamento == "S") {
                var valorTotalPagamentos = 0M;
                listaTituloReceitaPagamento.ForEach(item => {
                    valorTotalPagamentos += item.valorOriginal;
                });

                listaTituloReceitaPagamento.LastOrDefault().valorOriginal += UtilNumber.toDecimal(valorTotal - valorTotalPagamentos);
            }

            var ViewModel = new ReceitaCadastroForm();
            ViewModel.TituloReceita = new TituloReceita();
            ViewModel.TituloReceita.listaTituloReceitaPagamento = listaTituloReceitaPagamento;

            return View(ViewModel);
        }

        [HttpPost, ActionName("partial-dados-pagamento")]
        public PartialViewResult partialDadosPagamento(ReceitaCadastroForm ViewModel) {

            return PartialView(ViewModel);
        }

        [HttpPost, ActionName("partial-dados-pagamento-parcelado")]
        public PartialViewResult partialDadosPagamentoParcelado(ReceitaCadastroForm ViewModel) {

            return PartialView(ViewModel);
        }
    }
}