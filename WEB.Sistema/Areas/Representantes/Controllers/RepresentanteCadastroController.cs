using System;
using System.Web.Mvc;
using DAL.Representantes;
using WEB.App_Infrastructure;
using BLL.Representantes;
using DAL.Pessoas;
using MvcFlashMessages;
using WEB.Areas.Representantes.ViewModels;

namespace WEB.Areas.Representantes.Controllers {

    [OrganizacaoFilter]
    public class RepresentanteCadastroController : BaseSistemaController {

        //Constantes

        //Atributos
        private IRepresentanteConsultaBL _IRepresentanteConsultaBL;
        private IRepresentanteCadastroBL _IRepresentanteCadastroBL;

        //Propriedades
        private IRepresentanteConsultaBL ORepresentanteConsultaBL => _IRepresentanteConsultaBL = _IRepresentanteConsultaBL ?? new RepresentanteConsultaBL();
        private IRepresentanteCadastroBL ORepresentanteCadastroBL => _IRepresentanteCadastroBL = _IRepresentanteCadastroBL ?? new RepresentanteCadastroBL();
        
        //
        [HttpGet, ActionName("modal-cadastro")]
        public ActionResult modalCadastro(int? id) {

            var ViewModel = new RepresentanteForm();
            
            ViewModel.Representante = this.ORepresentanteConsultaBL.carregar(id.toInt()) ?? new Representante() { Pessoa = new Pessoa() };
            
            return View(ViewModel);
        }
        
        //
        [HttpPost, ValidateInput(false)]
        public ActionResult salvar(RepresentanteForm ViewModel) {

            if(!ModelState.IsValid) {
                return View("modal-cadastro", ViewModel);
            }
            
            bool flagSucesso = this.ORepresentanteCadastroBL.salvar(ViewModel.Representante);

            if(flagSucesso) {
                
                return Json(new { error = false });
                
            }
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");

            return View("modal-cadastro", ViewModel);
        }
        
        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.ORepresentanteCadastroBL.alterarStatus(id), JsonRequestBehavior.AllowGet);
        }

    }
    
}

