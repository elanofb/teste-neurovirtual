using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Caches;
using DAL.Permissao;
using BLL.Permissao;
using DAL.Permissao.Security.Extensions;

namespace WEB.App_Infrastructure {

    public class SecurityConfig {

        //Constantes
        private static SecurityConfig _instance;

        //Atributos
        private IAcessoPermissaoBL _AcessoPermissaoBL;

        //Propriedades
        private IAcessoPermissaoBL OAcessoPermissaoBL { get { return (this._AcessoPermissaoBL = this._AcessoPermissaoBL ?? new AcessoPermissaoBL()); } }

        public static SecurityConfig getInstance { get { return (_instance = _instance ?? new SecurityConfig()); } }

        //Construtor
        private SecurityConfig() {
        }

        //Metodos de validacao do login
        //public JsonMessage autenticarUsuario(string username, string password) {

        //	var ValidacaoLogin = OUsuarioSistemaBL.logar(username, password);

        //	if (ValidacaoLogin.error) {
        //		return ValidacaoLogin;
        //	}

        //	var Usuario = ValidacaoLogin.extraInfo as UsuarioSistemaVW;

        //	//setSessions(Usuario);
        //	return ValidacaoLogin;
        //}

        //Verificar a autenticacao
        //public bool isAuthenticate() {
        //	var User = SessionSistema.getUser();
        //	if (User != null){
        //		return true;
        //	}

        //	if(UtilConfig.flagProducao == "N"){
        //		UsuarioDesenvolvimento();
        //		return true;
        //	}

        //	return false;
        //}

        //Verificar permissao para acessar a área em questão
        public bool verificarAutorizacao(HttpContextBase httpContext) {

            int idOrganizacaoLogada = HttpContextFactory.Current.User.idOrganizacao();

            var listaRecursos = CacheService.getInstance.carregarSemOrganizacao<List<RecursoSistemaVW>>(CacheService.LISTA_RECURSOS);

            var listaPermissoes = CacheService.getInstance.carregar<List<RecursoPermissaoVW>>(CacheService.LISTA_PERMISSOES, idOrganizacaoLogada);

            if (listaRecursos == null) {

                listaRecursos = capturarRecursos(httpContext);

            }

            if (listaPermissoes == null || HttpContextFactory.Current.User.flagMultiOrganizacao()) {

                listaPermissoes = capturarPermissoes(httpContext);
            }

            if (listaPermissoes == null || listaRecursos == null) {

                //this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Seu login não possui configuração de permissões ou a sessão expirou.");

                return false;
            }

            string areaName = UtilString.notNull(httpContext.Request.RequestContext.RouteData.DataTokens["area"]).ToLower();

            string controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();

            string actionName = httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();

            int idPerfilLogado = httpContext.User.idPerfil();

            return verificarAutorizacao(idPerfilLogado, areaName, controllerName, actionName, listaRecursos, listaPermissoes);
        }


        /// <summary>
        /// Verificar a permissao de acordo com o local que está sendo acesso e as permissoes concedidas para o perfil
        /// </summary>
        private bool verificarAutorizacao(int idPerfilLogado, string areaName, string controllerName, string actionName, List<RecursoSistemaVW> listaRecursos, List<RecursoPermissaoVW> listaPermissoes) {

            areaName = areaName.stringOrEmptyLower();

            controllerName = controllerName.stringOrEmptyLower();

            actionName = actionName.stringOrEmptyLower();

            //Checar se a controller está cadastrada
            var subListaRecursos = listaRecursos.Where(
                                                x =>
                                                    (x.areaRecurso == areaName || x.areaAcao == areaName) &&
                                                    (x.controllerRecurso == controllerName || x.controllerAcao == controllerName ||
                                                     x.controllerGrupo == controllerName)
                                                ).ToList();

            //Checar se o action está protegido
            if (!String.IsNullOrEmpty(actionName)) {

                subListaRecursos = subListaRecursos.Where(x => (x.nomeAcao == actionName || x.actionPadrao == actionName || x.actionGrupo == actionName)).ToList();

            }

            //Se não houver cadastro de controller e action, liberar acesso
            if (subListaRecursos.Count == 0) {
                return true;
            }

            var subListaPermissao = listaPermissoes.Where(x =>
                                                       (x.idPerfilAcesso == idPerfilLogado) &&
                                                       (x.areaRecurso == areaName || x.areaAcao == areaName) &&
                                                       (x.controllerAcao == controllerName || x.controllerRecurso == controllerName)
                                                        ).ToList();

            if (subListaPermissao.Count == 0) {
                return false;
            }

            subListaPermissao = subListaPermissao.Where(x => (x.actionPadraoRecurso == actionName || x.nomeAcao == actionName)).ToList();

            if (subListaPermissao.Count == 0) {
                return false;
            }

            return true;
        }


        //Verificar permissao para acessar a área em questão
        public bool verificarAutorizacao(int idPerfilLogado, string actionName, string controllerName, string areaName) {

            int idOrganizacaoLogada = HttpContextFactory.Current.User.idOrganizacao();

            var listaRecursos = CacheService.getInstance.carregarSemOrganizacao<List<RecursoSistemaVW>>(CacheService.LISTA_RECURSOS);

            var listaPermissoes = CacheService.getInstance.carregar<List<RecursoPermissaoVW>>(CacheService.LISTA_PERMISSOES, idOrganizacaoLogada);

            if (listaRecursos == null) {

                listaRecursos = capturarRecursos(HttpContextFactory.Current);

            }

            if (listaPermissoes == null || (HttpContextFactory.Current.User.flagMultiOrganizacao() && idPerfilLogado != PerfilAcessoConst.DESENVOLVEDOR)) {

                listaPermissoes = capturarPermissoes(HttpContextFactory.Current);
            }

            if (listaPermissoes == null || listaRecursos == null) {

                //this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Seu login não possui configuração de permissões ou a sessão expirou.");

                return false;
            }

            return verificarAutorizacao(idPerfilLogado, areaName, controllerName, actionName, listaRecursos, listaPermissoes);
        }

        //Atualizar o cache da lista de Recursos
        private List<RecursoPermissaoVW> capturarPermissoes(HttpContextBase httpContext) {

            var OUser = HttpContextFactory.Current.User;
            
            int idOrganizacaoLogada = OUser.flagMultiOrganizacao() ? 0 : OUser.idOrganizacao();
            
            var listaPermissoes = OAcessoPermissaoBL.listarPermissoes(0, idOrganizacaoLogada).ToList();
            
            if (OUser.flagMultiOrganizacao() && OUser.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR){
                
                var idsOrganizacao = OUser.idsOrganizacoes();
                listaPermissoes = listaPermissoes.Where(x => idsOrganizacao.Contains(x.idOrganizacao.toInt())).ToList();   
            }            

            CacheService.getInstance.remover(CacheService.LISTA_PERMISSOES, idOrganizacaoLogada);

            CacheService.getInstance.adicionar(CacheService.LISTA_PERMISSOES, listaPermissoes, idOrganizacaoLogada);

            return listaPermissoes;
        }

        //Atualizar o cache da lista de Recursos
        private List<RecursoSistemaVW> capturarRecursos(HttpContextBase httpContext) {

            var listaRecursos = this.OAcessoPermissaoBL.listarRecursos().ToList();

            CacheService.getInstance.removerSemOrganizacao(CacheService.LISTA_RECURSOS);

            CacheService.getInstance.adicionarSemOrganizacao(CacheService.LISTA_RECURSOS, listaRecursos);

            return listaRecursos;
        }


    }
}