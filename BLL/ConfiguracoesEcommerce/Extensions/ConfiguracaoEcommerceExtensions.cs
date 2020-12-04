using BLL.ConfiguracoesEcommerce;
using DAL.ConfiguracoesEcommerce;

namespace System.Web.Mvc{

    public static class ConfiguracaoEcommerceExtensions{

        /// <summary>
        /// Carregar as configuracoes para area do associado
        /// </summary>
        public static ConfiguracaoEcommerce ecommerce(this HtmlHelper helper, int idOrganizacao) {

            var OConfig = ConfiguracaoEcommerceBL.getInstance.carregar(idOrganizacao);

            return OConfig;
        }


    }
}