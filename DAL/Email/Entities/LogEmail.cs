using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	//
	public partial class LogEmail {

		public int id { get; set; }

		public string idEmailServidor { get; set; }

		public int nroEmailServidor { get; set; }

		public string emailRemetente { get; set; }

		public string nomeRemetente { get; set; }

		public string emailResposta { get; set; }

		public string nomeResposta { get; set; }

		public string returnPath { get; set; }

		public string assunto { get; set; }

		public string corpoMensagem { get; set; }

		public string corpoEncode { get; set; }

		public string flagFluxo { get; set; }

		public string flagResposta { get; set; }

		public DateTime? dtEnvio { get; set; }

		public int? idUsuarioEnvio { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public DateTime? dtPrimeiraAbertura { get; set; }

		public int? idUsuarioPrimeiraAbertura { get; set; }

		public virtual List<LogEmailDestino> listaEmailDestino { get; set; }

		public LogEmail() {
			this.listaEmailDestino = new List<LogEmailDestino>();
		}
	}

	//
	internal sealed class LogEmailMapper : EntityTypeConfiguration<LogEmail> {

		public LogEmailMapper() {
			this.ToTable("syslog_email");
			this.HasKey(o => o.id);
		}
	}
}