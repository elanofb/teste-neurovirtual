using System;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using MvcFlashMessages;
using WEB.Areas.FinanceiroLancamentos.ViewModels;
using WEB.App_Infrastructure;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class DespesaCloneController : BaseSistemaController {

        // Atributos
        private ITituloDespesaCloneFacadeBL _ITituloDespesaCloneFacadeBL;
        
        // Propriedades
        private ITituloDespesaCloneFacadeBL OTituloDespesaCloneFacadeBL => _ITituloDespesaCloneFacadeBL = _ITituloDespesaCloneFacadeBL ?? new TituloDespesaCloneFacadeBL();
        
        //
        [ActionName("modal-clonar-despesa")]
        public ActionResult modalClonarDespesa(int id) {
            
            var ViewModel = new DespesaCloneForm();
            
            ViewModel.carregarDespesaBase(id);

            if (ViewModel.TituloDespesa == null) {

                return Json(new { error = false, message = "A despesa informada não foi encontrada." }, JsonRequestBehavior.AllowGet);
            }
            
            return View(ViewModel);
        }
    
        //
        [HttpPost, ActionName("clonar-despesa")]
        public ActionResult clonarDespesa(DespesaCloneForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-clonar-despesa", ViewModel);
            }

            var array = ViewModel.idReferenciaPessoa.Split('#');
            
            ViewModel.TituloDespesa.flagCategoriaPessoa = array[0];
            
            ViewModel.TituloDespesa.idPessoa = Convert.ToInt32(array[1]);
            
            var ORetorno = this.OTituloDespesaCloneFacadeBL.clonar(ViewModel.TituloDespesa, ViewModel.qtdeReplicacoes);

            if (!ORetorno.flagError) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "A despesa foi replicada com sucesso."));

                var idPrimeiraDespesaGerada = ORetorno.info.toInt();
                
                var urlRedirect = Url.Action("editar", "DespesaDetalhe", new { Area = "Financeiro", id = idPrimeiraDespesaGerada });

                return Json(new { error = false, urlRedirect }, JsonRequestBehavior.AllowGet);
            }

            return View("modal-clonar-despesa", ViewModel);
        }

    }
    
}
