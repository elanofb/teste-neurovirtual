using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using AutoMapper;
using DAL.Repository.Base;
using FluentValidation.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Arquivos.Config.Mapper;
using WEB.Areas.Empresas.Config.Mapper;
using WEB.Areas.Permissao.Config.Mapper;
using WEB.Areas.Publicacoes.Config.Mapper;
using System.Globalization;
using System.Net;
using System.Threading;
using WEB.App_Infrastructure.Core;

namespace WEB {
    public class MvcApplication : HttpApplication {

        //Atributos
        //private BackgroundJobServer _backgroundJobServer;

        //Propriedades

        //Inicio da Aplicacao
        protected void Application_Start() {

            //
            this.loadAppStart();

            //
            this.initCulture();

            //
            this.initDatabase();

            //
            this.initModules();

            //
            this.initLibs();

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }

        /// <summary>
        /// Arquivos de App_Start
        /// </summary>
        private void loadAppStart() {

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SincViewEngine());

            AppDiContainer.register();

            DefaultModelBinder.ResourceClassKey = "Messages";
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Messages";
        }

        //Configurar as culturas padrão
        private void initCulture() {
            var cultureInfo = new CultureInfo("pt-BR");

            cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        ///Iniciar metodos de configurações dos módulos
        private void initModules() {

            Mapper.Initialize(cfg => {

                cfg.AddProfile(new PermissaoProfile());

                cfg.AddProfile(new ArquivosProfile());

                cfg.AddProfile(new EmpresaProfile());
                
                cfg.AddProfile(new PublicacoesProfile());

                // Área Associado


            });

        }

        //Inicialização da base de dados
        private void initDatabase() {
            Database.SetInitializer<DataContext>(null);
            Database.SetInitializer<RelatorioImediatoContext>(null);
            //DbInterception.Add(new DataContextInterceptor());
        }
        
        // 1- Iniciar Fluent Validation - Usando nas validacoes MVC e de ViewModels
        private void initLibs() {
            FluentValidationModelValidatorProvider.Configure();
        }

        // 1 - Configurar storage padrao para armazenamento das informaçoes das tarefas
        // 2 - Iniciar o servidor dos Jobs Hangfire
        // 3 - Chamar as classes responsaveis por armazenar as tarefas
        //      private void initTasks() {

        //	//GlobalConfiguration.Configuration.UseSqlServerStorage("STDefaultConnection");
        //	//_backgroundJobServer = new BackgroundJobServer();

        //	//TarefaDiariaBL.getInstance.executar();
        //	//TarefaRecorrenteBL.getInstance.executar();
        //}

        //Evento para início de requisicao
        protected void Application_BeginRequest() {
            if (Request.HttpMethod == "OPTIONS") {
                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.AppendHeader("Access-Control-Allow-Origin", "*");
                Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                Response.AppendHeader("Access-Control-Allow-Credentials", "true");
                Response.End();
            }
        }


        //      //Evento para fim de requisicao
        //      protected void Application_EndRequest() {
        //      }


        //Inicializacao de sessoes
        protected void Session_Start() {

            Session["init"] = 0;

        }

        //      //Evento no encerramento de sessoes
        //      protected void Session_End() {
        //          //UtilLog.saveLog("Sessão finalizada.", "session");
        //      }

        ////Evento para finalizacao da aplicacao
        //      protected void Application_End() {
        //	//_backgroundJobServer.Dispose();
        //      }

    }
}
