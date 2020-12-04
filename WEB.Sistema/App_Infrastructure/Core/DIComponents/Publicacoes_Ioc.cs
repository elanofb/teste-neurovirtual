using BLL.Publicacoes;
using SimpleInjector;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class Publicacoes_IoC {
        
        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {
            
            container.Register<INoticiaBL, NoticiaBL>();

        }          
    }

}
