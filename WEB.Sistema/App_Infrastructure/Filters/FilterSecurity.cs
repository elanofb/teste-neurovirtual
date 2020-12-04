using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WEB.App_Infrastructure;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;
using SecurityExtensions = DAL.Permissao.Security.Extensions.SecurityExtensions;

namespace WEB {
	
	public class FilterSecurity : AuthorizeAttribute {
        
        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            
			if (SecurityExtensions.hasLogin(httpContext.User)) {
                return true;
            }
            return false;
        }

	    //
		public override void OnAuthorization(AuthorizationContext filterContext) {

			var OUser = filterContext.HttpContext.User;


			int idPerfilLogado = OUser.idPerfil();

			//
			if (idPerfilLogado == PerfilAcessoConst.DESENVOLVEDOR) {
				return;
			}

			//Se houver filtro de anônimo na action
			if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()) {
				return;
			}

			//Se houver filtro de anônimo na controller
			if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()) {
				return;
			}


			//Caso seja uma action filha liberar o acesso
			if (filterContext.IsChildAction) {

				base.OnAuthorization(filterContext);

                return;
			}


			string areaName = UtilString.notNull(filterContext.RouteData.DataTokens["area"]);
			string controllerName = filterContext.RouteData.Values["controller"].ToString();
			string actionName = filterContext.RouteData.Values["action"].ToString();
			string method = filterContext.HttpContext.Request.HttpMethod;

			if (controllerName.StartsWith("login") || controllerName.StartsWith("erro")) {
				base.OnAuthorization(filterContext);
				return;
			}

			if (!SecurityExtensions.hasLogin(OUser)) {

				base.OnAuthorization(filterContext);

			    if (filterContext.HttpContext.Request.IsAjaxRequest()){

				    filterContext.Result = new RedirectToRouteResult(
					    new RouteValueDictionary {
					    { "area", "Erros" },
					    { "controller", "Erro" },
					    { "action", "login-expirado" },
					    { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
				    });

			        return;
			    }

				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary {
					{ "area", "permissao" },
					{ "controller", "login" },
					{ "action", "index" },
					{ "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
				});

				
				return;
			}

			if (!filterContext.HttpContext.Request.IsAjaxRequest() && OUser.flagAlterarSenha() == "S" && (controllerName != "usuariosistemaacesso" && actionName != "alterar-senha")) {

				base.OnAuthorization(filterContext);

				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary {
					{ "area", "permissao" },
					{ "controller", "usuariosistemaacesso" },
					{ "action", "alterar-senha" },
					{ "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
				});

				
				return;
			}

			bool flagAutorizado = SecurityConfig.getInstance.verificarAutorizacao(filterContext.HttpContext);

			if (!flagAutorizado) {

				UtilLog.accessDenied(areaName, controllerName, actionName);
				
				base.OnAuthorization(filterContext);

				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary {
					{ "area", "Erros" },
					{ "controller", "erro" },
					{ "action", "error403" },
					{ "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
				});
			}
		}
	}
}
