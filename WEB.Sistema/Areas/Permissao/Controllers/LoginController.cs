using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Configuracoes;
using WEB.Areas.Permissao.ViewModels;
using BLL.Permissao;
using DAL.Permissao;
using MvcFlashMessages;
using BLL.LogsPermissao;
using BLL.UsuariosUnidades;
using DAL.Permissao.Security.Extensions;
using BLL.Organizacoes;
using DAL.Configuracoes;

namespace WEB.Areas.Permissao.Controllers {

    [AllowAnonymous]
    public class LoginController : Controller {

		//Constantes

		//Atributos
		private UsuarioSistemaBL _UsuarioSistemaBL;
        private IUsuarioSistemaAcessoBL _UsuarioSistemaAcessoBL;
        private IUsuarioUnidadeBL _UsuarioUnidadeBL;
        private LogUsuarioSistemaAcessoBL _LogUsuarioSistemaAcessoBL;
        private IConfiguracaoSistemaBL _IConfiguracaoSistemaBL;
        private IUsuarioOrganizacaoBL _UsuarioOrganizacaoBL;
        
        // Propriedades
        private UsuarioSistemaBL OUsuarioSistemaBL => (this._UsuarioSistemaBL = this._UsuarioSistemaBL ?? new UsuarioSistemaBL());
		private IUsuarioSistemaAcessoBL OUsuarioSistemaAcessoBL => (this._UsuarioSistemaAcessoBL = this._UsuarioSistemaAcessoBL ?? new UsuarioSistemaAcessoBL());
        private IUsuarioUnidadeBL OUsuarioUnidadeBL => this._UsuarioUnidadeBL = this._UsuarioUnidadeBL ?? new UsuarioUnidadeBL();
        private IUsuarioOrganizacaoBL OUsuarioOrganizacaoBL => this._UsuarioOrganizacaoBL = this._UsuarioOrganizacaoBL ?? new UsuarioOrganizacaoBL();
        private LogUsuarioSistemaAcessoBL OLogUsuarioSistemaAcessoBL => (this._LogUsuarioSistemaAcessoBL = this._LogUsuarioSistemaAcessoBL ?? new LogUsuarioSistemaAcessoBL());
        private IConfiguracaoSistemaBL OConfiguracaoSistemaBL => _IConfiguracaoSistemaBL = _IConfiguracaoSistemaBL ?? new ConfiguracaoSistemaBL();

        // GET
        [HttpGet, ActionName("index-custom")]
        public ActionResult indexCustom(string rotaCustomizada, string returnUrl) {

            if (rotaCustomizada.Equals("index") || rotaCustomizada.Equals("sair")) {
                return RedirectToAction("index");
            }
            
            ConfiguracaoSistema OConfigSistema = null;

            if (!rotaCustomizada.isEmpty()) {

                OConfigSistema = this.OConfiguracaoSistemaBL.listar(0).FirstOrDefault(x => x.rotaCustomizadaLogin.Equals(rotaCustomizada));
            }
            
            if (OConfigSistema == null) {

                var dominio = HttpContextFactory.Current.Request.Url?.Host;

                OConfigSistema = this.OConfiguracaoSistemaBL.listar(0).FirstOrDefault(x => x.dominios.Contains(dominio)) ?? this.OConfiguracaoSistemaBL.carregarPadrao();
            }

            if (OConfigSistema != null) {

                User.setOrganizacao(OConfigSistema.idOrganizacao.ToString());
            }
	        
	        if (!returnUrl.isEmpty()) {

		        return Redirect(returnUrl);
	        }
            
            return RedirectToAction("index");

        }

        //GET
        [HttpGet]
		public ActionResult index(string returnUrl) {

            var ViewModel = new LoginVM();

            if (User.idOrganizacao() == 0) { 

                var dominio = HttpContextFactory.Current.Request.Url?.Host;

                var OConfigSistema = this.OConfiguracaoSistemaBL.listar(0).FirstOrDefault(x => x.dominios.Contains(dominio));

                if (OConfigSistema?.idOrganizacao > 0) { 

                    User.setOrganizacao(OConfigSistema.idOrganizacao.ToString());

                    ViewModel.OConfigSistema = OConfigSistema;

                    return RedirectToAction("index");

                }

            }

            ViewModel.OConfigSistema = ViewModel.OConfigSistema ?? this.OConfiguracaoSistemaBL.carregar();

            this.deslogar();

            ViewModel.returnUrl = returnUrl;
            
			return View(ViewModel);
		}

		//POST
		[HttpPost]
		public ActionResult index(LoginVM ViewModel){ 

			if (!ModelState.IsValid) {

				return PartialView(ViewModel);
			}
			
			var ValidacaoLogin = OUsuarioSistemaAcessoBL.login(ViewModel.login, ViewModel.senha);

			if (ValidacaoLogin.flagError) {

				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Acesso Negado!", ValidacaoLogin.listaErros.FirstOrDefault()));

				return PartialView(ViewModel);

			}

			var OUsuario = (ValidacaoLogin.info as UsuarioSistema);

			if (OUsuario == null) {

				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Acesso Negado!", ValidacaoLogin.listaErros.FirstOrDefault()) );

				return PartialView(ViewModel);

			}
            
		    OUsuario = this.OUsuarioSistemaBL.carregar(OUsuario.id);

			User.signInFromEntity(OUsuario);

            if (OUsuario.PerfilAcesso.flagTodasUnidades != true) {

                var queryUnidadesAcesso = this.OUsuarioUnidadeBL.listar(OUsuario.id, 0);

                var listaIdsUnidadesAcesso = queryUnidadesAcesso.Select(x => x.idUnidade).ToArray();

                User.signUnidades(listaIdsUnidadesAcesso);

                if (!User.flagTodasUnidades()) {

                    var OUnidadeLogada = queryUnidadesAcesso.FirstOrDefault();

                    User.setUnidade(OUnidadeLogada.idUnidade.ToString(), OUnidadeLogada.Unidade.sigla);
                }
            }

		    if (OUsuario.PerfilAcesso.flagOrganizacao == true) {
			    
                var queryOrganizacoesAcesso = this.OUsuarioOrganizacaoBL.listar(OUsuario.id, 0);

                var listaIdsOrganizacoessAcesso = queryOrganizacoesAcesso.Select(x => x.idOrganizacao).ToList();
		        if (listaIdsOrganizacoessAcesso.All(x => x != OUsuario.idOrganizacao)) {
                    listaIdsOrganizacoessAcesso.Add(OUsuario.idOrganizacao ?? 0);
                }

                User.signOrganizacoes(listaIdsOrganizacoessAcesso.ToArray());
            }

            ViewModel.LogUsuariosistemaAcesso.idUsuario = OUsuario.id;

            this.OLogUsuarioSistemaAcessoBL.salvar(HttpContext, ViewModel.LogUsuariosistemaAcesso);

            string urlRedirecionamento = String.IsNullOrEmpty(ViewModel.returnUrl)? Url.Action("index", "home", new {area = ""}) : ViewModel.returnUrl;

			return Json(new {error = false, urlRedirecionamento });

		}

		//GET
		[HttpGet]
		[ActionName("recuperar-senha")]
		public ActionResult recuperarSenha(){

            var ViewModel = new LoginVM();

            if (User.idOrganizacao() == 0) { 

                var dominio = HttpContextFactory.Current.Request.Url?.Host;

                var OConfigSistema = this.OConfiguracaoSistemaBL.listar(0).FirstOrDefault(x => x.dominios.Contains(dominio));

                if (OConfigSistema?.idOrganizacao > 0) { 

                    User.setOrganizacao(OConfigSistema.idOrganizacao.ToString());

                    ViewModel.OConfigSistema = OConfigSistema;

                    return RedirectToAction("index");

                }
            }

            ViewModel.OConfigSistema = ViewModel.OConfigSistema ?? this.OConfiguracaoSistemaBL.carregar();

            this.deslogar();
            
			return View(ViewModel);
		}

		//POST
		[HttpPost]
		[ActionName("recuperar-senha")]
		public ActionResult recuperarSenha(string login){

			var Retorno = this.OUsuarioSistemaAcessoBL.recuperarSenha(login);

			if (Retorno.flagError) { 

				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, Retorno.listaErros.FirstOrDefault());
				return RedirectToAction("recuperar-senha");
			}

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, Retorno.listaErros.FirstOrDefault());

			return RedirectToAction("index");
		}

		//GET
		[HttpGet]
		public ActionResult sair(){ 

			this.deslogar();
			
			return RedirectToAction("index");
		}

		//
	    private void deslogar() {

            CacheService.getInstance.limparCacheOrganizacao(User.idOrganizacao());

            User.signOut();
            
	    }
    }
}