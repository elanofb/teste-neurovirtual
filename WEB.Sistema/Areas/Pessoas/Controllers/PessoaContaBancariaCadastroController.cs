using System;
using System.Web.Mvc;

using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.App_Infrastructure;

// ReSharper disable Mvc.ViewNotResolved
// ReSharper disable Mvc.PartialViewNotResolved
namespace WEB.Areas.Pessoas.Controllers {
    
    public class PessoaContaBancariaCadastroController : BaseSistemaController {
          
        [HttpGet, ActionName("modal-cadastro-conta")]      
        public ActionResult modalCadastroConta(int idPessoa) {
            
            
            return View();            

        }


    }
}