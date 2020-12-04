using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Financeiro.Controllers {

    public class TituloReceitaController : Controller {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private ITituloReceitaBL OTituloReceitaBL => (_TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL());

		//Abrir o modal com formulario para registro da data de pagamento
		[HttpGet, ActionName("modal-detalhe-pagamento")]
        public ActionResult modalDetalhePagamento(int id) {

		    var OTitulo = this.OTituloReceitaBL.carregar(id) ?? new TituloReceita();

            var ViewModel = new ModalDetalhePagamento();

            ViewModel.carregarDados(OTitulo);

		    ViewModel.ParcelaAdicional.valorOriginal = ViewModel.valorDiferencaParcelas;

			return PartialView(ViewModel);
        }

		//salvar o pagamento de uma parcela com baixa manual
		[HttpPost, ActionName("salvar-ajuste-parcelas")]
        public ActionResult salvarAjusteParcelas(ModalDetalhePagamento ViewModel) {

		    var OTitulo = this.OTituloReceitaBL.carregar(ViewModel.TituloReceita.id);

            ViewModel.carregarDados(OTitulo);

		    if (ViewModel.ParcelaAdicional.valorOriginal == 0) {

		        ModelState.AddModelError("ParcelaAdicional.valorOriginal", "Informe um valor válido para a parcela.");

		    }

		    if (ViewModel.ParcelaAdicional.valorOriginal < 1) {

		        ModelState.AddModelError("ParcelaAdicional.valorOriginal", $"A parcela não pode ter um valor menor do que {new decimal(1).ToString("C")}.");

		    }

		    ViewModel.valorParcelado = Decimal.Add(ViewModel.valorParcelado, ViewModel.ParcelaAdicional.valorOriginal);

            if (ViewModel.valorParcelado > OTitulo.valorTotal) {

		        ModelState.AddModelError("ParcelaAdicional.valorOriginal", $"Valor inválido! a soma do parcelamento não deve ser maior do que {OTitulo.valorTotal.exibirValor()}.");

		    }

            if (ViewModel.ParcelaAdicional.dtVencimento < DateTime.Today) {

		        ModelState.AddModelError("ParcelaAdicional.dtVencimento", "Informe uma data válida para vencimento.");
		    }

            if (!ModelState.IsValid) {

                return PartialView("partial-form-ajustar-parcelas", ViewModel);

            }

		    var listaPagamentos = new List<TituloReceitaPagamento>();

            ViewModel.ParcelaAdicional.idUsuarioCadastro = User.id();

            ViewModel.ParcelaAdicional.idUsuarioAlteracao = User.id();

            listaPagamentos.Add(ViewModel.ParcelaAdicional);

            //this.OTituloReceitaBL.salvarParcelas(OTitulo, listaPagamentos, false);

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O parcela foi registrada com sucesso!");

			return Json(new {error = false, message="", OTitulo.id});

        }

    }
}