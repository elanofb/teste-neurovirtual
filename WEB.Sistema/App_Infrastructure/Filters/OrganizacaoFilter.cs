using System;
using System.Web.Mvc;
using System.Web.Routing;
using DAL.Permissao.Security.Extensions;

namespace WEB {
	
	public class OrganizacaoFilter : AuthorizeAttribute {

		/// <summary>
        /// Verificar se o usuario logado tem uma organizacao vinculada
        /// </summary>
        /// <param name="filterContext"></param>
		public override void OnAuthorization(AuthorizationContext filterContext) {

			var OUser = HttpContextFactory.Current.User;

            //Se for requisição Ajax, liberar acesso
            if (OUser.idOrganizacao() > 0) {
				return;
			}
				
			base.OnAuthorization(filterContext);

			filterContext.Result = new RedirectToRouteResult(
				new RouteValueDictionary {
				{ "area", "Erros" },
				{ "controller", "erro" },
				{ "action", "sem-organizacao" },
				{ "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
			});

		}

		
	}
}
