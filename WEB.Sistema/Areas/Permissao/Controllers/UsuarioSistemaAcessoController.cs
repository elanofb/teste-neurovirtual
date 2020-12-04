using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Permissao.ViewModels;
using WEB.App_Infrastructure;
using BLL.Permissao;
using MvcFlashMessages;
using DAL.Permissao.Security.Extensions;
using DAL.Permissao.Security;

namespace WEB.Areas.Permissao.Controllers{

    public class UsuarioSistemaAcessoController : BaseSistemaController{

		//Constantes

		//Atributos
        private IUsuarioSistemaAcessoBL _UsuarioSistemaAcessoBL;
        private IUsuarioSistemaBL _UsuarioSistemaBL;

		//Propriedades
		private IUsuarioSistemaAcessoBL OUsuarioSistemaAcessoBL => this._UsuarioSistemaAcessoBL = this._UsuarioSistemaAcessoBL ?? new UsuarioSistemaAcessoBL();
        private IUsuarioSistemaBL OUsuarioSistemaBL => this._UsuarioSistemaBL = this._UsuarioSistemaBL ?? new UsuarioSistemaBL();

        //Reenvio de senha por e-maol
        public ActionResult reenviarSenha() {

			int idUsuarioSistema = UtilRequest.getInt32("idUsuarioSistema");

            var Retorno = this.OUsuarioSistemaAcessoBL.criarNovaSenha(idUsuarioSistema);

            return Json(new { error = Retorno.flagError, message=Retorno.listaErros.FirstOrDefault()}, JsonRequestBehavior.AllowGet);
        }

        //GET
		[HttpGet, ActionName("alterar-senha")]
        public ActionResult alterarSenha(){

            var ViewModel = new AlterarSenhaForm();

            return View(ViewModel);
        }

        //
		[ActionName("alterar-senha"), HttpPost]
        public ActionResult alterarSenha(AlterarSenhaForm ViewModel){

            int idUsuario = User.id();
            
			if (!ModelState.IsValid) {
	            return View(ViewModel);
			}

            var Retorno = this.OUsuarioSistemaBL.alterarSenha(idUsuario, ViewModel.senha);

            if (!Retorno.flagError) {
					
				this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Sua senha foi alterada com sucesso.");

	            SecurityCookie.flagAlterarSenha = "N";
                
				return RedirectToAction("index", "home", new{area=""});
            }
            

            return View(ViewModel);
        }

    }
}
