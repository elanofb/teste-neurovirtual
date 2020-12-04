using System;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using WEB.App_Infrastructure;
using MvcFlashMessages;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.FinanceiroParcelamentos.ViewModels;

namespace WEB.Areas.FinanceiroParcelamentos.Controllers {

    public class TituloReceitaParcelamentoController : BaseSistemaController {

        //Atributos
        private ITituloReceitaConsultaBL _TituloReceitaConsultaBL;

        //Propriedades
        private ITituloReceitaConsultaBL OTituloReceitaConsultaBL => _TituloReceitaConsultaBL = _TituloReceitaConsultaBL ?? new TituloReceitaConsultaBL();


        //Abrir o modal com formulario para registro de parcelamento da anuidade
        [HttpGet, ActionName("modal-parcelar-titulo")]
        public ActionResult modalParcelarTitulo(int id) {

            var ViewModel = new TituloReceitaParcelamentoForm();

            ViewModel.carregarTitulo(id, User.idOrganizacao());

            if (ViewModel.TituloReceita.id == 0 || ViewModel.TituloReceita.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "A cobrança não foi localizada.");

                return PartialView(ViewModel);

            }

            if (ViewModel.TituloReceita.valorTotalComDesconto() <= 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível parcelar uma cobrança sem valor.");

                return PartialView(ViewModel);
                
            }

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Carregar o formulario com a quantidade de parcelas escolhida
        /// </summary>
        [ActionName("partial-carregar-parcelas")]
        public ActionResult partialCarregarParcelas(int id, int? qtdeParcelas) {

            var ViewModel = new TituloReceitaParcelamentoForm();

            ViewModel.carregarTitulo(id, User.idOrganizacao());

            ViewModel.qtdeParcelas = qtdeParcelas.toInt();

            if (ViewModel.TituloReceita.id == 0 || ViewModel.TituloReceita.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "A cobrança não foi localizada.");

                return PartialView("partial-form-parcelas", ViewModel);

            }

            if (ViewModel.qtdeParcelas.toInt() == 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe uma quantidade de parcelas válida.");

                return PartialView("partial-form-parcelas", ViewModel);

            }
            
            if (ViewModel.TituloReceita.valorTotalComDesconto() <= 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível parcelar uma cobrança sem valor.");

                return PartialView("partial-form-parcelas", ViewModel);
                
            }            

            ViewModel.carregarParcelas();

            return PartialView("partial-form-parcelas", ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-parcelas")]
        public ActionResult salvarParcelas(TituloReceitaParcelamentoForm ViewModel) {

            ViewModel.TituloReceita = OTituloReceitaConsultaBL.carregar(ViewModel.TituloReceita.id) ?? new TituloReceita();

            ViewModel.carregarParcelas();

            if (ViewModel.TituloReceita.id == 0 || ViewModel.TituloReceita.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "A cobrança não foi localizada.");

                return PartialView("partial-form-parcelas", ViewModel);

            }

            if (ViewModel.listaPagamentos.Count < 2) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe uma quantidade de parcelas válida.");

                return PartialView("partial-form-parcelas", ViewModel);

            }

            if (ViewModel.TituloReceita.valorTotalComDesconto() <= 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível parcelar uma cobrança sem valor.");

                return PartialView("partial-form-parcelas", ViewModel);
                
            }  
            
            if (!ModelState.IsValid) {

                return PartialView("partial-form-parcelas", ViewModel);

            }

            var Retorno = new UtilRetorno();// this.OTituloReceitaParcelamentoBL.registrarParcelamento(ViewModel.TituloReceita, ViewModel.listaPagamentos);

            if (!Retorno.flagError) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O parcelamemento foi realizado com sucesso.");
            }

            return Json(new { error = Retorno.flagError, message = string.Join("<br />", Retorno.listaErros) });
        }
    }
}
