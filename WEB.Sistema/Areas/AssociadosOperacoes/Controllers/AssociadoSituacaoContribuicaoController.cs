using System;
using System.Web.Mvc;
using BLL.AssociadosOperacoes;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers  {

    public class AssociadoSituacaoContribuicaoController : Controller {
        
        // Atributos
        private IAssociadoSituacaoContribuicaoBL _IAssociadoSituacaoContribuicaoBL;

        // Propriedades
        private IAssociadoSituacaoContribuicaoBL OAssociadoSituacaoContribuicaoBL => _IAssociadoSituacaoContribuicaoBL = _IAssociadoSituacaoContribuicaoBL ?? new AssociadoSituacaoContribuicaoBL();

        [ActionName("modal-alteracao")]
        public ActionResult modalAlteracao(int id) {
             
            var ViewModel = new AssociadoSituacaoContribuicaoForm();
            ViewModel.id = id;

            return View(ViewModel);
        }

        [HttpPost, ActionName("salvar-alteracao")]
        public ActionResult salvarAlteracao(AssociadoSituacaoContribuicaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-alteracao", ViewModel);
            }

            this.OAssociadoSituacaoContribuicaoBL.alterarSituacaoContribuicao(ViewModel.id, ViewModel.motivoAlteracao);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "A alteração foi realizada com sucesso."));

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

        }

    }

}