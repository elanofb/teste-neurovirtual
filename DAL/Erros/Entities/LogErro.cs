using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Erros {

	//
	public class LogErro {

		public int id { get; set; }

		public int? idUsuarioLogado { get; set; }

		public DateTime dtErro { get; set; }

		public string exceptionMessage { get; set; }

		public string exceptionTrace { get; set; }

		public string exceptionInnerMessage { get; set; }

		public string exceptionInnerTrace { get; set; }

		public string source { get; set; }

		public string metodo { get; set; }

		public string url { get; set; }

		public string module { get; set; }

		public string controllerName { get; set; }

		public string actionName { get; set; }

		public string ip { get; set; }
	}

	//
	internal sealed class LogErroMapper : EntityTypeConfiguration<LogErro> {

		public LogErroMapper() {
			this.ToTable("syslog_erros");
			this.HasKey(o => o.id);
		}
	}
}