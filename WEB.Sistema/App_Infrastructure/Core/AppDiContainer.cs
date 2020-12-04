using System.Web.Http;
using System.Web.Mvc;
using DAL.Repository.Base;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using WEB.App_Infrastructure.Core.DIComponents;


namespace WEB.App_Infrastructure.Core {
    
    public class AppDiContainer {
        
        //Atributos
        private Container container { get; }
        //Propriedades

        /// <summary>
        /// Registro de dependencias
        /// </summary>
        public static void register() {
         
            var container = new Container();
            
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            //Core
            container.Register<DataContext, DataContext>(Lifestyle.Scoped);
               
            //Notificacoes     
            UTIL_IoC.mapear(ref container);

            //Organizacoes
            Organizacoes_IoC.mapear(ref container);

            //Configuracoes
            Configuracoes_IoC.mapear(ref container);
            
           
            //Notificacoes     
            Notificacoes_IoC.mapear(ref container);
            
            //Arquivos/Arquivo Upload
            Arquivos_IoC.mapear(ref container);
            
            //Financeiro
            Financeiro_IoC.mapear(ref container);
            
            //Publicacoes
            Publicacoes_IoC.mapear(ref container);
            
            //Registrar controllers WEB API
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            
            //Check and resolve
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

    }
        
}
