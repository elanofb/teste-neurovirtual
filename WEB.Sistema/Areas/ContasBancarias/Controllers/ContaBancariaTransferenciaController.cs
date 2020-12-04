using System;
using System.Web.Mvc;
using BLL.ContasBancarias;
using DAL.ContasBancarias;
using WEB.App_Infrastructure;
using WEB.Areas.ContasBancarias.ViewModels;

namespace WEB.Areas.ContasBancarias.Controllers {

    [OrganizacaoFilter]
    public class ContaBancariaTransferenciaController : BaseSistemaController {
        
        //Atributos
        private IContaBancariaBL _IContaBancariaBL;
        private IContaBancariaMovimentacaoBL _IContaBancariaMovimentacaoBL;

        //Propriedades
        private IContaBancariaBL OContaBancariaBL => _IContaBancariaBL = _IContaBancariaBL ?? new ContaBancariaBL();
        private IContaBancariaMovimentacaoBL OContaBancariaMovimentacaoBL => _IContaBancariaMovimentacaoBL = _IContaBancariaMovimentacaoBL ?? new ContaBancariaMovimentacaoBL();

        //
        [ActionName("modal-realizar-transferencia")]
        public ActionResult modalRealizarTransferencia(int idContaBancaria) {

            var ViewModel = new ContaBancariaTransferenciaForm();

            ViewModel.ContaBancariaMovimentacao = new ContaBancariaMovimentacao() { dtOperacao = DateTime.Today };

            ViewModel.ContaBancariaMovimentacao.ContaBancariaOrigem = this.OContaBancariaBL.carregar(idContaBancaria);

            if (ViewModel.ContaBancariaMovimentacao.ContaBancariaOrigem == null) {

                return Json(new { flagErro = false, message = "A conta bancária informada não foi encontrada" });

            }

            ViewModel.ContaBancariaMovimentacao.idContaBancariaOrigem = idContaBancaria;

            return View(ViewModel);

        }

        //
        [HttpPost, ActionName("salvar-movimentacao")]
        public ActionResult salvarMovimentacao(ContaBancariaTransferenciaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-realizar-transferencia", ViewModel);
            }

            ViewModel.ContaBancariaMovimentacao.idTipoOperacao = ContaTipoOperacaoConst.TRANSFERENCIA;

            var flagSucesso = this.OContaBancariaMovimentacaoBL.salvar(ViewModel.ContaBancariaMovimentacao);

            if (flagSucesso) {

                return Json(new { error = false, message = "A transferência foi realizada com sucesso." }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { error = true, message = "Houve algum problema ao realizar a transferência. Tente novamente mais tarde." }, JsonRequestBehavior.AllowGet);

        }

    }

}