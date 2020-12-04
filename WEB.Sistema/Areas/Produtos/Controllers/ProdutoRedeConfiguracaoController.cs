using System;
using System.Web.Mvc;
using BLL.Produtos;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.Produtos.ViewModels;

namespace WEB.Areas.Produtos.Controllers {

    public class ProdutoRedeConfiguracaoController : BaseSistemaController {
        
        //Atributos
        private IProdutoRedeConfiguracaoCadastroBL _CadastroBL;
        
        //Servicoes
        private IProdutoRedeConfiguracaoCadastroBL CadastroBL => _CadastroBL = _CadastroBL ?? new ProdutoRedeConfiguracaoCadastroBL();
         
        // GET
        [ActionName("partial-form-configuracao")]
        public ActionResult partialFormConfiguracao() {

            var ViewModel = new ProdutoRedeForm();

            ViewModel.ProdutoRede.idProduto = UtilRequest.getInt32("idProduto");
        
            return PartialView(ViewModel);
        }
        
        // GET
        [HttpPost]
        public ActionResult salvar(ProdutoRedeForm ViewModel) {

            if (!ModelState.IsValid) {
                
                return PartialView("partial-form-configuracao", ViewModel);
            }

            var flagSucesso = this.CadastroBL.salvar(ViewModel.ProdutoRede);

            if (flagSucesso) {

                return Json(new {error = false, message = ""});
            }
            

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Não foi possível realizar todas as operações necessárias."));
        
            return PartialView("partial-form-configuracao", ViewModel);
        }        
    }

}
