using System;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.DadosBancarios.ViewModels;

namespace WEB.Areas.DadosBancarios.Controllers {
    
    public class DadosBancariosConsultaController : BaseSistemaController {

        [HttpGet, ActionName("partial-listar")]
        public PartialViewResult partialListar(int idPessoa){
            
            var ViewModel = new DadosBancariosConsultaVM();
            ViewModel.idPessoa = idPessoa;
            ViewModel.montarLista();
            
            return PartialView(ViewModel);
            
        }
        
        [ActionName("listar-json")]
        public ActionResult listarJson(int idPessoa){
            
            var ViewModel = new DadosBancariosConsultaVM();
            ViewModel.idPessoa = idPessoa;
            ViewModel.montarLista();

            var lista = ViewModel.listaDadosBancarios.Where(x => x.ativo == true).Select(x => new {
                            value = x.id,
                            text = String.Concat(x.Banco.descricao, " - ", x.nroConta)
                        }).ToList();
            
            return Json(new { error = false, lista }, JsonRequestBehavior.AllowGet);
            
        }
        
    }
}