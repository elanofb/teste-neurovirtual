using BLL.Arquivos;
using SimpleInjector;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class Arquivos_IoC {
        
        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {

            container.Register<IArquivoUploadCadastroBL, ArquivoUploadCadastroBL>();
            container.Register<IGravadorArquivoBL, GravadorArquivoBL>();
        }          
    }

}
