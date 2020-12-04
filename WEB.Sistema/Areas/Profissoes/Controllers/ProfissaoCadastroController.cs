using System;
using System.Web.Mvc;
using DAL.Profissoes;
using WEB.App_Infrastructure;
using BLL.Profissoes;
using MvcFlashMessages;
using WEB.Areas.Profissoes.ViewModels;

namespace WEB.Areas.Profissoes.Controllers {

    public class ProfissaoCadastroController : BaseSistemaController {

        //Constantes

        //Atributos
        private IProfissaoCadastroBL _IProfissaoCadastroBL;
        private IProfissaoConsultaBL _IProfissaoConsultaBL;
        
        //Propriedades
        private IProfissaoCadastroBL OProfissaoCadastroBL => _IProfissaoCadastroBL = _IProfissaoCadastroBL ?? new ProfissaoCadastroBL();
        private IProfissaoConsultaBL OProfissaoConsultaBL => _IProfissaoConsultaBL = _IProfissaoConsultaBL ?? new ProfissaoConsultaBL();
        
        // GET: EventosPro/EventoCadastro
        [ActionName("modal-form-cadastro")]
        public ActionResult modalFormCadastro(int? id) {
			
            var ViewModel = new ProfissaoForm();

            ViewModel.OProfissao = this.OProfissaoConsultaBL.carregar(id.toInt()) ?? new Profissao();
			
            return View(ViewModel);
        }
		
        //POST
        [HttpPost, ActionName("salvar-profissao")]
        public ActionResult salvarEvento(ProfissaoForm ViewModel) {

            if(!ModelState.IsValid) {
                return PartialView("modal-form-cadastro", ViewModel);
            }
            
            bool flagSucesso = this.OProfissaoCadastroBL.salvar(ViewModel.OProfissao);

            if (flagSucesso) {

                //this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O processo de avaliação foi salvo com sucesso.");

                return Json(new { error = false, message = "" });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houver algum problema ao tentar salvar os dados do evento.");

            return PartialView("modal-form-cadastro", ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OProfissaoCadastroBL.alterarStatus(id));
        }

    }
}
