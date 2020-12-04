using System;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using MvcFlashMessages;
using WEB.Areas.FinanceiroLancamentos.ViewModels;
using WEB.App_Infrastructure;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    [OrganizacaoFilter]
    public class ReceitaCloneController : BaseSistemaController {

        // Atributos
        private ITituloReceitaCloneFacadeBL _ITituloReceitaCloneFacadeBL;
        
        // Propriedades
        private ITituloReceitaCloneFacadeBL OTituloReceitaCloneFacadeBL => _ITituloReceitaCloneFacadeBL = _ITituloReceitaCloneFacadeBL ?? new TituloReceitaCloneFacadeBL();
        
        //
        [ActionName("modal-clonar-Receita")]
        public ActionResult modalClonarReceita(int id) {
            
            var ViewModel = new ReceitaCloneForm();
            
            ViewModel.carregarReceitaBase(id);

            if (ViewModel.TituloReceita == null) {

                return Json(new { error = false, message = "A receita informada não foi encontrada." }, JsonRequestBehavior.AllowGet);
            }
            
            return View(ViewModel);
        }
    
        //
        [HttpPost, ActionName("clonar-receita")]
        public ActionResult clonarReceita(ReceitaCloneForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-clonar-receita", ViewModel);
            }

            var array = ViewModel.idReferenciaPessoa.Split('#');
            
            ViewModel.TituloReceita.flagCategoriaPessoa = array[0];
            
            ViewModel.TituloReceita.idPessoa = Convert.ToInt32(array[1]);
            
            var ORetorno = this.OTituloReceitaCloneFacadeBL.clonar(ViewModel.TituloReceita, ViewModel.qtdeReplicacoes);

            if (!ORetorno.flagError) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "A Receita foi replicada com sucesso."));

                var idPrimeiraReceitaGerada = ORetorno.info.toInt();
                
                var urlRedirect = Url.Action("editar", "ReceitaDetalhe", new { Area = "Financeiro", id = idPrimeiraReceitaGerada });

                return Json(new { error = false, urlRedirect }, JsonRequestBehavior.AllowGet);
            }

            return View("modal-clonar-receita", ViewModel);
        }

    }
    
}
