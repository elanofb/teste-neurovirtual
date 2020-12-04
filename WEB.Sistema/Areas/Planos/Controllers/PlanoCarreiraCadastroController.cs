using System;
using System.Web.Mvc;
using BLL.Planos;
using DAL.Planos;
using MvcFlashMessages;
using WEB.Areas.Planos.ViewModels;

namespace WEB.Areas.Planos.Controllers{
    
    public class PlanoCarreiraCadastroController : Controller{
        //Atributos
        private IPlanoCarreiraCadastroBL _PlanoCarreiraCadastroBL;
        private IPlanoCarreiraConsultaBL _PlanoCarreiraConsultaBL;

        //Propriedades
        private IPlanoCarreiraCadastroBL OPlanoCarreiraCadastroBL => this._PlanoCarreiraCadastroBL = this._PlanoCarreiraCadastroBL ?? new PlanoCarreiraCadastroBL();
        private IPlanoCarreiraConsultaBL OPlanoCarreiraConsultaBL => this._PlanoCarreiraConsultaBL = this._PlanoCarreiraConsultaBL ?? new PlanoCarreiraConsultaBL();

        [HttpGet]
        public ActionResult index(int? id){
            
            var ViewModel = new PlanoCarreiraForm();
            ViewModel.PlanoCarreira = this.OPlanoCarreiraConsultaBL.carregar(UtilNumber.toInt32(id)) ?? new PlanoCarreira();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult salvar(PlanoCarreiraForm ViewModel){
            if (!ModelState.IsValid)
                return View("index", ViewModel);

            bool flagSucesso = this.OPlanoCarreiraCadastroBL.salvar(ViewModel.PlanoCarreira);
            
            if (flagSucesso){
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
                return RedirectToAction("index", new{ViewModel.PlanoCarreira.id});
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            return View("index", ViewModel);
        }
        
        //
        /*[ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {

            var ORetorno = this.OPlanoCarreiraBL.alterarStatus(id);

            if (!ORetorno.error) {
                CacheService.getInstance.limparCacheOrganizacao(null, PlanoCarreiraBL.keyCache);
            }

            return Json(ORetorno);
        }*/
    }
}