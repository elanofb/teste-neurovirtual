using System.Linq;
using System.Web.Mvc;
using BLL.LogsAlteracoes;
using BLL.Planos;
using DAL.Entities;
using WEB.Areas.Planos.ViewModels;

namespace WEB.Areas.Planos.Controllers{
    
    public class PlanoCarreiraAlteracaoDadosListaController : Controller {
        
        //Atributos
        private IPlanoCarreiraConsultaBL _IPlanoCarreiraConsultaBL;
        private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedades
        private IPlanoCarreiraConsultaBL OPlanoCarreiraConsultaBL => _IPlanoCarreiraConsultaBL = _IPlanoCarreiraConsultaBL ?? new PlanoCarreiraConsultaBL();
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();

        //
        [HttpGet, ActionName("modal-logs-plano")]
        public ActionResult modalLogsPlanoCarreira(int idPlanoCarreira) {
                
            var OPlanoCarreira = this.OPlanoCarreiraConsultaBL.carregar(idPlanoCarreira);
                            
            if (OPlanoCarreira == null) {
                return Json(new { flagError = true, message = "O plano informado não foi encontrado" }, JsonRequestBehavior.AllowGet);
            }
            
            var ViewModel = new PlanoCarreiraAlteracaoDadosListaVM();

            ViewModel.idPlanoCarreira = idPlanoCarreira;
            
            ViewModel.listaLogs = this.OLogAlteracaoBL.listar(EntityTypesConst.PLANO_CARREIRA, idPlanoCarreira, "")
                                            .OrderByDescending(x => x.id).ToList();

            return View(ViewModel); 

        }    
        
        //
        [HttpPost, ActionName("buscar-logs")]
        public PartialViewResult buscarLogs(PlanoCarreiraAlteracaoDadosListaVM ViewModel) {
            
            ViewModel.listaLogs = this.OLogAlteracaoBL.listar(EntityTypesConst.PLANO_CARREIRA, ViewModel.idPlanoCarreira, ViewModel.valorBusca)
                                            .OrderByDescending(x => x.id).ToList();
            
            return PartialView("partial-lista-logs", ViewModel.listaLogs);

        }

    }

}