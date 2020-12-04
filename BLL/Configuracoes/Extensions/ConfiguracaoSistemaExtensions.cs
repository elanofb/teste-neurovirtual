using System.Linq;
using DAL.Configuracoes;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;

namespace System.Web.Mvc{

    public static class ConfiguracaoSistemaExtensions {

        /// <summary>
        /// Carregar as configuracoes do sistema
        /// </summary>
        public static ConfiguracaoSistema sistema(this HtmlHelper helper) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var OConfig = ConfiguracaoSistemaBL.getInstance.carregar(idOrganizacao);

            return OConfig;
        }

        /// <summary>
        /// Carregar as configuracoes do sistema
        /// </summary>
        public static ConfiguracaoSistema sistema(this HtmlHelper helper, int idOrganizacao) {

            var OConfig = ConfiguracaoSistemaBL.getInstance.carregar(idOrganizacao);

            return OConfig;
        }
        
    }
    
}