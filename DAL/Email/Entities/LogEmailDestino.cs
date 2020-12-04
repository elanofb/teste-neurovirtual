using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	//
	public partial class LogEmailDestino {

		public int id { get; set; }

		public int idEmail { get; set; }

		public virtual LogEmail Email { get; set; }

		public string emailDestino { get; set; }

		public string nomeDestino { get; set; }

		public string flagCopia { get; set; }

		public string flagCopiaOculta { get; set; }

		public DateTime dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class LogEmailDestinoMapper : EntityTypeConfiguration<LogEmailDestino> {

		public LogEmailDestinoMapper() {
			this.ToTable("syslog_email_destino");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Email).WithMany(e => e.listaEmailDestino).HasForeignKey(o => o.idEmail);
		}
	}
}