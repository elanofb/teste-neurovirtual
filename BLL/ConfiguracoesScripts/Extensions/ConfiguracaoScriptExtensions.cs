using BLL.ConfiguracoesScripts;
using DAL.Permissao.Security.Extensions;
using DAL.ConfiguracoesScripts;

namespace System.Web.Mvc{

    public static class ConfiguracaoScriptExtensions{

        /// <summary>
        /// Carregar as configuracoes do sistema
        /// </summary>
        public static ConfiguracaoScripts scripts(this HtmlHelper helper) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var OConfig = ConfiguracaoScriptsBL.getInstance.carregar(idOrganizacao);

            return OConfig;
        }
        
    }
}