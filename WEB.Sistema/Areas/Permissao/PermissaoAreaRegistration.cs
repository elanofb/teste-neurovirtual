using System.Web.Mvc;

namespace WEB.Areas.Permissao {
	public class PermissaoAreaRegistration : AreaRegistration {

		public override string AreaName => "Permissao";

	    public override void RegisterArea(AreaRegistrationContext context) {

			 context.MapRoute(
				name: "PerfilAcesso_Permissao",
				url: "perfil-acesso/{action}",
				defaults: new { controller = "perfilacesso", AreaName = "permissao" },
				namespaces: new[] { "WEB.Areas.Permissao.Controllers" }
			);

			 context.MapRoute(
				name: "UsuarioSistema_Permissao",
				url: "usuariosistema/{action}",
				defaults: new { controller = "usuariosistema", AreaName = "permissao" },
				namespaces: new[] { "WEB.Areas.Permissao.Controllers" }
			);

	        context.MapRoute(
	            "Login_Customizado",
	            "Login/{rotaCustomizada}",
	            new { AreaName = "Permissao", controller = "Login", action = "index-custom" },
	            new[] { "WEB.Areas.Permissao.Controllers" }
	        );

            context.MapRoute(
				"Login_Default",
				"Login",
				new { AreaName = "Permissao", controller = "Login", action = "index" },
				new[] { "WEB.Areas.Permissao.Controllers" }
			);
            
			context.MapRoute(
				"Permissao_default",
				"Permissao/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "WEB.Areas.Permissao.Controllers" }
			);
		}
	}
}