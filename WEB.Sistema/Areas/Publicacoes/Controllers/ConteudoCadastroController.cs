using System;
using System.Web.Mvc;
using BLL.Publicacoes;
using DAL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using MvcFlashMessages;
using WEB.App_Infrastructure;

namespace WEB.Areas.Publicacoes.Controllers {

    [OrganizacaoFilter]
    public class ConteudoCadastroController : BaseSistemaController {
        
        //Atributos
        private IConteudoConsultaBL _ConteudoConsultaBL;
        private IConteudoCadastroBL _ConteudoCadastroBL;
        
        //Propriedades
        private IConteudoConsultaBL OConteudoConsultaBL => _ConteudoConsultaBL = _ConteudoConsultaBL ?? new ConteudoConsultaBL();
        private IConteudoCadastroBL OConteudoCadastroBL => _ConteudoCadastroBL = _ConteudoCadastroBL ?? new ConteudoCadastroBL();
        
        //Construtor
        public ConteudoCadastroController() {

        }             
        
        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new ConteudoForm();
            
            ViewModel.Conteudo = this.OConteudoConsultaBL.carregar(UtilNumber.toInt32(id)) ?? new Conteudo();
            
            return View(ViewModel);
        }
        
        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConteudoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }                        
            
            bool flagSucesso = this.OConteudoCadastroBL.salvar(ViewModel.Conteudo);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
                return RedirectToAction("editar", new { id = ViewModel.Conteudo.id });
            }    
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            
            return View(ViewModel);
        }
        
        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OConteudoCadastroBL.alterarStatus(id));
        }
        
    }
}