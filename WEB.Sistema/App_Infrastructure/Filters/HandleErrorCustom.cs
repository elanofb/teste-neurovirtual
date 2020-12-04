using System;
using System.Web;
using System.Web.Mvc;
using DAL.Permissao.Security.Extensions;
using Newtonsoft.Json;

namespace WEB.App_Infrastructure {

    public class HandleErrorCustom : HandleErrorAttribute {

        //Intercepta as exceções    

        public override void OnException(ExceptionContext filterContext) {

            var User = HttpContextFactory.Current.User;

            
            try {

                var OErroDTO = new {
                    dominio = HttpContext.Current.Request.Url.Host,
                    url = HttpContext.Current.Request.RawUrl,
                    dtErro = DateTime.Now,
                    exceptionMessage = filterContext.Exception.Message,
                    exceptionInnerMessage = filterContext.Exception.InnerException?.Message,
                    exceptionTrace = filterContext.Exception.StackTrace,
                    metodo = filterContext.Exception.TargetSite.ToString(),
                    ip = HttpContextFactory.Current.Request.UserHostAddress,
                    idUsuarioLogado = DAL.Permissao.Security.Extensions.SecurityExtensions.id(HttpContextFactory.Current.User)
                };

                UtilLog.saveError(filterContext.Exception, "");

                if (UtilConfig.emProducao()) {
                    string dados = JsonConvert.SerializeObject(OErroDTO);
                    string jsonRetorno = UtilHTTP.postSync(UtilConfig.linkAbsLogErro, dados);
                }

            } catch (Exception ex) {
                UtilLog.saveError(ex, "");
            }

            base.OnException(filterContext);
        }
    }
}
