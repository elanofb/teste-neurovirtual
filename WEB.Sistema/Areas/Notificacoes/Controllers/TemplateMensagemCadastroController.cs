using System;
using System.Web.Mvc;
using BLL.Notificacoes;
using MvcFlashMessages;
using WEB.Areas.Notificacoes.ViewModels;

namespace WEB.Areas.Notificacoes.Controllers {

    public class TemplateMensagemCadastroController : Controller {

        //Atributos
        private ITemplateMensagemCadastroBL _TemplateMensagemCadastroBL;

        //Propriedades
        private ITemplateMensagemCadastroBL OTemplateMensagemCadastroBL => this._TemplateMensagemCadastroBL = this._TemplateMensagemCadastroBL ?? new TemplateMensagemCadastroBL();

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            TemplateMensagemForm ViewModel = new TemplateMensagemForm();

            ViewModel.carregar(id.toInt());

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(TemplateMensagemForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            this.OTemplateMensagemCadastroBL.salvar(ViewModel.TemplateMensagem);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { ViewModel.TemplateMensagem.id });
        }
        
        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OTemplateMensagemCadastroBL.alterarStatus(id));
        }
    }
}