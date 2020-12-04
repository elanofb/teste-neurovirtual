using System;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using UTIL;

namespace DAL.Repository.Base {

	public class DataContextInterceptor : DbCommandInterceptor {
		private ILogger _logger = new Logger();

		private delegate void ExecutingMethod<T>(System.Data.Common.DbCommand command, DbCommandInterceptionContext<T> interceptionContext);

		public override void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext) {
			CommandExecuting<int>(base.NonQueryExecuting, command, interceptionContext);
		}

		public override void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext) {
			CommandExecuting<System.Data.Common.DbDataReader>(base.ReaderExecuting, command, interceptionContext);
		}

		public override void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {
			CommandExecuting<object>(base.ScalarExecuting, command, interceptionContext);
		}

		private void CommandExecuting<T>(ExecutingMethod<T> executingMethod, System.Data.Common.DbCommand command, DbCommandInterceptionContext<T> interceptionContext) {
			Stopwatch sw = Stopwatch.StartNew();
			executingMethod.Invoke(command, interceptionContext);
			sw.Stop();

			if (interceptionContext.Exception != null) {
				_logger.error(interceptionContext.Exception, String.Format("Erro ao executar o comando: {0}", command.CommandText));
			} else {
				_logger.information(String.Format("{0} took {1}", command.CommandText, sw.Elapsed.ToString()));
			}
		}
	}
}