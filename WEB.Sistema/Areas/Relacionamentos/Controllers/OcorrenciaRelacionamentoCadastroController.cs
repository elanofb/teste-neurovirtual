using System;
using System.Web.Mvc;
using BLL.Relacionamentos;
using DAL.Relacionamentos;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.Relacionamentos.ViewModels;

namespace WEB.Areas.Relacionamentos.Controllers {
    
    [OrganizacaoFilter]
    public class OcorrenciaRelacionamentoCadastroController : BaseSistemaController {
        
        // Atributos
        private IOcorrenciaRelacionamentoConsultaBL _IOcorrenciaRelacionamentoConsultaBL;
        private IOcorrenciaRelacionamentoCadastroBL _IOcorrenciaRelacionamentoCadastroBL;

        // Propriedades
        private IOcorrenciaRelacionamentoConsultaBL OOcorrenciaRelacionamentoConsultaBL => _IOcorrenciaRelacionamentoConsultaBL = _IOcorrenciaRelacionamentoConsultaBL ?? new OcorrenciaRelacionamentoConsultaBL();
        private IOcorrenciaRelacionamentoCadastroBL OOcorrenciaRelacionamentoCadastroBL => _IOcorrenciaRelacionamentoCadastroBL = _IOcorrenciaRelacionamentoCadastroBL ?? new OcorrenciaRelacionamentoCadastroBL();

        //
        [HttpGet, ActionName("modal-form-cadastro")]
        public ActionResult modalFormCadastro(int? id) {

            var ViewModel = new OcorrenciaRelacionamentoCadastroForm();

            ViewModel.OcorrenciaRelacionamento = this.OOcorrenciaRelacionamentoConsultaBL.carregar(id.toInt()) ?? new OcorrenciaRelacionamento();

            ViewModel.idComboSelecionar = UtilRequest.getString("idComboSelecionar");

            ViewModel.flagRecarregar = UtilRequest.getBool("flagRecarregar") ?? false;

            return View(ViewModel);
        }
        
        //
        [HttpPost]
        public ActionResult salvar(OcorrenciaRelacionamentoCadastroForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-form-cadastro", ViewModel);    
            }

            var flagSucesso = this.OOcorrenciaRelacionamentoCadastroBL.salvar(ViewModel.OcorrenciaRelacionamento);

            if (flagSucesso) {

                if (ViewModel.flagRecarregar) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados do tipo de contato foram salvos com sucesso."));
                }
                
                return Json(new { error = false, ViewModel.flagRecarregar, ViewModel.idComboSelecionar, ViewModel.OcorrenciaRelacionamento.id, ViewModel.OcorrenciaRelacionamento.descricao }, JsonRequestBehavior.AllowGet);
                
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao salvar o tipo de contato."));
            
            return View("modal-form-cadastro", ViewModel);
        }
        
        //
        [ActionName("alterar-status")]
        public JsonResult alterarStatus(int id) {
            return Json(this.OOcorrenciaRelacionamentoCadastroBL.alterarStatus(id), JsonRequestBehavior.AllowGet);
        }

    }
    
}