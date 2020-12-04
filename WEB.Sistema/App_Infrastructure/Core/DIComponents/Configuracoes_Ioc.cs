using BLL.Configuracoes;
//using BLL.ArquivosRemessa.Services.Vendors.RemessasDespesa.ItauCnab240.Configuracoes;
using SimpleInjector;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class Configuracoes_IoC {

        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {

            //container.RegisterConditional(typeof(ILeitorConfiguracao), typeof(LeitorConfiguracaoCnab240), Lifestyle.Singleton, x => x.Consumer.ImplementationType.Namespace.Contains("ItauCnab240"));
            
        }
    }
}
