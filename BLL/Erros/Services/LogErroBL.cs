using System;
using System.Linq;
using System.Web;
using BLL.Services;
using DAL.Erros;
using DAL.Permissao.Security.Extensions;

namespace BLL.Erros {

	public class LogErroBL : DefaultBL {

	    public IQueryable<LogErro> listar() {
	        var query = from LE in db.LogErro select LE;

	        return query;
	    } 

		//
		public void save(Exception ex) {
			LogErro Erro = new LogErro();

			Erro.idUsuarioLogado = User.id();
			Erro.exceptionMessage = ex.Message;
			Erro.exceptionTrace = ex.StackTrace;
			if (ex.InnerException != null) {
				Erro.exceptionInnerMessage = ex.InnerException.Message;
				Erro.exceptionInnerTrace = ex.InnerException.StackTrace;
				if (ex.InnerException.InnerException != null) {
					Erro.exceptionInnerMessage = ex.InnerException.InnerException.Message;
					Erro.exceptionInnerTrace = ex.InnerException.InnerException.StackTrace;
				}
			}
			Erro.dtErro = DateTime.Now;
			Erro.ip = System.Web.HttpContext.Current.Request.UserHostAddress;
			Erro.source = ex.Source;
			Erro.url = HttpContext.Current.Request.RawUrl;
			Erro.metodo = ex.TargetSite.ToString();

			var rd = HttpContext.Current.Request.RequestContext.RouteData;
			if (rd != null) {
				Erro.module = rd.Values["area"] as string;
				Erro.controllerName = rd.GetRequiredString("controller");
				Erro.actionName = rd.GetRequiredString("action");
			}

            Erro.setDefaultInsertValues<LogErro>();
            db.LogErro.Add(Erro);
            db.SaveChanges();

			UtilLog.saveError(ex, "");
		}
	}
}