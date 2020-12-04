using DAL.Permissao;
using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.LogsAlteracoes {

	//
	public partial class LogAlteracao {

		public int id { get; set; }

		public int idEntidadeReferencia { get; set; }

		public int idReferencia { get; set; }

        public string nomeCampoAlterado { get; set; }

        public string valorAntigo { get; set; }

        public string valorNovo { get; set; }

        public string justificativa { get; set; }
        
		public string ip { get; set; }

		public string nomeCampoDisplay { get; set; }
		public string oldValueSelect { get; set; }
		public string newValueSelect { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public virtual UsuarioSistema UsuarioAlteracao { get; set; }

        public DateTime dtAlteracao { get; set; }
    }

	//
	internal sealed class LogAlteracaoMapper : EntityTypeConfiguration<LogAlteracao> {

		public LogAlteracaoMapper() {
			this.ToTable("syslog_alteracao");
			this.HasKey(o => o.id);
            this.HasRequired(x => x.UsuarioAlteracao).WithMany().HasForeignKey(x => x.idUsuarioAlteracao);
		}
	}
}