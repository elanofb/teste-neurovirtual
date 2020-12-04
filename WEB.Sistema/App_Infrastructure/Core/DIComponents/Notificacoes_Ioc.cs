using BLL.AssociadosOperacoes;
using BLL.Emails;
using BLL.Notificacoes;
using BLL.Notificacoes.Interface;
using SimpleInjector;
using WEB.Areas.AssociadosOperacoes.Controllers;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class Notificacoes_IoC {
        
        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {
            
            container.Register<INotificacaoSistemaConsultaBL, NotificacaoSistemaConsultaBL>();
            container.Register<INotificadorTask, NotificadorTask>();
            container.Register<INotificador, Notificador>();
            
            container.Register<INotificacaoSistemaCadastroBL, NotificacaoSistemaCadastroBL>();
            container.Register<IMensagemEmailConsultaBL, MensagemEmailConsultaBL>();

        }          
    }

}
