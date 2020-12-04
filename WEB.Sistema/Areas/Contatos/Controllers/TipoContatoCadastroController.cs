using BLL.Contatos;
using System;
using System.Web.Mvc;
using DAL.Contatos;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.Contatos.ViewModels;

namespace WEB.Areas.Contatos.Controllers {
    
    [OrganizacaoFilter]
    public class TipoContatoCadastroController : BaseSistemaController {
        
        // Atributos
        private ITipoContatoConsultaBL _ITipoContatoConsultaBL;
        private ITipoContatoCadastroBL _ITipoContatoCadastroBL;

        // Propriedades
        private ITipoContatoConsultaBL OTipoContatoConsultaBL => _ITipoContatoConsultaBL = _ITipoContatoConsultaBL ?? new TipoContatoConsultaBL();
        private ITipoContatoCadastroBL OTipoContatoCadastroBL => _ITipoContatoCadastroBL = _ITipoContatoCadastroBL ?? new TipoContatoCadastroBL();

        //
        [HttpGet, ActionName("modal-form-cadastro")]
        public ActionResult modalFormCadastro(int? id) {

            var ViewModel = new TipoContatoForm();

            ViewModel.TipoContato = this.OTipoContatoConsultaBL.carregar(id.toInt()) ?? new TipoContato();

            ViewModel.idComboSelecionar = UtilRequest.getString("idComboSelecionar");

            ViewModel.flagRecarregar = UtilRequest.getBool("flagRecarregar") ?? false;

            return View(ViewModel);
        }
        
        //
        [HttpPost]
        public ActionResult salvar(TipoContatoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-form-cadastro", ViewModel);    
            }

            var flagSucesso = this.OTipoContatoCadastroBL.salvar(ViewModel.TipoContato);

            if (flagSucesso) {

                if (ViewModel.flagRecarregar) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados do tipo de contato foram salvos com sucesso."));
                }
                
                return Json(new { error = false, ViewModel.flagRecarregar, ViewModel.idComboSelecionar, ViewModel.TipoContato.id, ViewModel.TipoContato.descricao }, JsonRequestBehavior.AllowGet);
                
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao salvar o tipo de contato."));
            
            return View("modal-form-cadastro", ViewModel);
        }
        
        //
        [ActionName("alterar-status")]
        public JsonResult alterarStatus(int id) {
            return Json(this.OTipoContatoCadastroBL.alterarStatus(id), JsonRequestBehavior.AllowGet);
        }

    }
    
}