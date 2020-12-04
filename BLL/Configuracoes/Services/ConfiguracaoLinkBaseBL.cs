using System;
using System.Linq;
using System.Net;
using DAL.Permissao.Security.Extensions;

namespace BLL.Configuracoes {

    public static class ConfiguracaoLinkBaseBL {

        /// <summary>
        /// Carregar o link base do sistema 
        /// </summary>
        public static string linkSistema(int? idOrganizacaoParam = null, string complementoUrl = "") {

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = HttpContextFactory.Current.User.idOrganizacao();
            }

            var linkBase = "";
            
            var OConfig = ConfiguracaoSistemaBL.getInstance.carregar(idOrganizacaoParam.toInt());

            if (!OConfig.dominios.isEmpty()) {
                
                string[] separadores = { "\r\n" };
                var listaDominios = OConfig.dominios.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToList();

                if (listaDominios.Any()) {

                    var primeiroDominio = listaDominios.FirstOrDefault();

                    linkBase = String.Concat(HttpContextFactory.Current.Request.Url?.Scheme, "://", primeiroDominio);

                    if (!complementoUrl.isEmpty()) {
                        
                        linkBase = String.Concat(linkBase, complementoUrl);
                    }
                    
                    return linkBase;
                }

            }

            if (!OConfig.rotaCustomizadaLogin.isEmpty()) {

                linkBase = $"{HttpContextFactory.Current.Request.Url?.Scheme}://{HttpContextFactory.Current.Request.Url?.Host}/Login/{OConfig.rotaCustomizadaLogin}";

                if (!complementoUrl.isEmpty()) {

                    var urlRedirect = $"{HttpContextFactory.Current.Request.Url?.Scheme}://{HttpContextFactory.Current.Request.Url?.Host}/{complementoUrl}";
                    
                    linkBase = linkBase + "?returnUrl=" + WebUtility.UrlEncode(urlRedirect);
                }
                
                return linkBase;
                
            }

            linkBase = $"{HttpContextFactory.Current.Request.Url?.Scheme}://{HttpContextFactory.Current.Request.Url?.Host}";
            
            if (!complementoUrl.isEmpty()) {

                linkBase = String.Concat(linkBase, complementoUrl);
            }
            
            return linkBase;
        }
    
        /// <summary>
        /// Carregar o link base da área do associado
        /// </summary>
        public static string linkAreaAssociado(int? idOrganizacaoParam = null, string complementoUrl = "", bool? flagRotaCustomizadaLogin = true) {

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = HttpContextFactory.Current.User.idOrganizacao();
            }
            
            var linkBase = "";
            
            var OConfig = ConfiguracaoSistemaBL.getInstance.carregar(idOrganizacaoParam.toInt());
            
            if (!OConfig.dominios.isEmpty()) {
                
                string[] separadores = { "\r\n" };
                var listaDominios = OConfig.dominios.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToList();

                if (listaDominios.Any()) {

                    var primeiroDominio = listaDominios.FirstOrDefault();

                    linkBase = String.Concat(HttpContextFactory.Current.Request.Url?.Scheme, "://", primeiroDominio, "/AreaAssociados/Home/Index/index-custom");

                    if (!complementoUrl.isEmpty()) {
                        
                        var urlRedirect = String.Concat(HttpContextFactory.Current.Request.Url?.Scheme, "://", primeiroDominio, "/AreaAssociados/", complementoUrl);
                        
                        linkBase = linkBase + "?returnUrl=" + WebUtility.UrlEncode(urlRedirect);
                    }

                    return linkBase;
                }

            }

            if (!OConfig.rotaCustomizadaLogin.isEmpty()){

                string rotaCustomizadaLogin = flagRotaCustomizadaLogin == true ? OConfig.rotaCustomizadaLogin : "";

                linkBase = $"{HttpContextFactory.Current.Request.Url?.Scheme}://{HttpContextFactory.Current.Request.Url?.Host}/AreaAssociados/{rotaCustomizadaLogin}";

                if (!complementoUrl.isEmpty()) {

                    var urlRedirect = $"{HttpContextFactory.Current.Request.Url?.Scheme}://{HttpContextFactory.Current.Request.Url?.Host}/AreaAssociados/{complementoUrl}";
                    
                    linkBase = linkBase + "?returnUrl=" + WebUtility.UrlEncode(urlRedirect);
                }

                return linkBase;
                
            }

            linkBase = $"{HttpContextFactory.Current.Request.Url?.Scheme}://{HttpContextFactory.Current.Request.Url?.Host}/AreaAssociados/";
            
            if (!complementoUrl.isEmpty()) {

                linkBase = String.Concat(linkBase, complementoUrl);
            }
            
            return linkBase;
        }

        /// <summary>
        /// Carregar o link de pré atualização de cadastro da área do associado
        /// </summary>
        public static string linkPreAtualizacaoAreaAssociado(int? idOrganizacaoParam, int? idAssociado, string emailOrigem) {
            
            if (idAssociado.toInt() == 0){
                return "";
            }
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = HttpContextFactory.Current.User.idOrganizacao();
            }
            
            // Preparação de parametros do link de atualização cadastral
            var parametroIdOrganizacaoB = String.Concat("ob=", UtilString.encodeURL(UtilCrypt.toBase64Encode(idOrganizacaoParam.toInt())));			
                  
            var parametroIdOrganizacaoS = String.Concat("os=", UtilString.encodeURL(UtilCrypt.SHA512(idOrganizacaoParam.ToString())));
                    
            var parametroIdAssociadoB = String.Concat("ab=", UtilString.encodeURL(UtilCrypt.toBase64Encode(idAssociado.toInt())));			
                  
            var parametroIdAssociadoS = String.Concat("as=", UtilString.encodeURL(UtilCrypt.SHA512(idAssociado.ToString())));
			        
            var parametroEmail = String.Concat("m=", UtilString.encodeURL(emailOrigem));
			        
            // Junção dos parâmetros formados
            var parametros = String.Concat("?", parametroIdOrganizacaoB, "&", parametroIdOrganizacaoS, "&", parametroIdAssociadoB, "&", parametroIdAssociadoS, "&", parametroEmail);
                        
            var linkEnvio = ConfiguracaoLinkBaseBL.linkAreaAssociado(idOrganizacaoParam, String.Concat("MinhaConta/PreAtualizacaoAutenticacao/", parametros));

            return linkEnvio;
            
        }
        
    }
    
}