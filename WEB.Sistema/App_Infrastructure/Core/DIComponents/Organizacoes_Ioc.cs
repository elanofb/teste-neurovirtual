using BLL.Organizacoes;
using SimpleInjector;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class Organizacoes_IoC {
        
        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {
            
            container.Register<IOrganizacaoBL, OrganizacaoBL>();

        }          
    }

}
