using System;
using System.Web.Mvc;
using BLL.Financeiro.Services;
using UTIL.UtilClasses;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class TituloDespesaPagamentoEdicaoController : Controller {

        //Atributos
        private ITituloDespesaPagamentoAlteracaoDadosBL _TituloDespesaPagamentoAlteracaoDadosBL;

        //Propriedades
        private ITituloDespesaPagamentoAlteracaoDadosBL OTituloDespesaPagamentoAlteracaoDadosBL => _TituloDespesaPagamentoAlteracaoDadosBL = _TituloDespesaPagamentoAlteracaoDadosBL ?? new TituloDespesaPagamentoAlteracaoDadosBL();
        
        //Carrega o detalhe do pagamento do titulo
        [HttpGet, ActionName("modal-edicao-pagamento")]
        public ActionResult modalEdicaoPagamento(int? id) {

            var ViewModel = new PagamentoDespesaDetalheVM();

            ViewModel.idTituloDespesaPagamento = id.toInt();
            
            ViewModel.flagEdicao = true;
            ViewModel.carregar();

            if (!(ViewModel.OPagamentoDespesa.id > 0)) {
                return Json(new { flagErro = true, message = "Não foi possível encontrar o titulo ou não é possível edita-lo." }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("~/Areas/Financeiro/Views/TituloDespesaPagamentoDetalhe/modal-detalhe-pagamento.cshtml", ViewModel);
        }
        
        [ActionName("alterar-dados"), HttpPost, ValidateInput(false)]
        public ActionResult alterarDados(EditableItem ViewModel){
            
            UtilRetorno ORetorno = new UtilRetorno();
            int idPagamento = ViewModel.pk.toInt();

            if (ORetorno.flagError == false){
                ORetorno = OTituloDespesaPagamentoAlteracaoDadosBL.alterarCampo(idPagamento, ViewModel.name, ViewModel.value, ViewModel.nomeCampoDisplay, ViewModel.oldValue, ViewModel.newValue);
            }
            
            if (!ViewModel.viewName.isEmpty() && ORetorno.flagError){

                return PartialView(ViewModel.viewName, ViewModel);
                
            }

            return Json(new { error = ORetorno.flagError, message = string.Join("<br />", ORetorno.listaErros), ViewModel.targetBox, ViewModel.value });
        }
        
    }

}
