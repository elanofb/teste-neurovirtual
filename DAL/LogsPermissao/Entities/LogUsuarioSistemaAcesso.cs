using DAL.Permissao;
using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.LogsPermissao {

	//
	public partial class LogUsuarioSistemaAcesso {

		public int id { get; set; }

		public int idUsuario { get; set; }

        public virtual UsuarioSistema UsuarioSistema { get; set; }

        public DateTime dtAcesso { get; set; }

		public string ipAcesso { get; set; }

		public string idSessao { get; set; }

		public string latitude { get; set; }

		public string longitude { get; set; }

		public string browser { get; set; }

        public string sistemaOperacional { get; set; }
    }

	//
	internal sealed class LogUsuarioSistemaAcessoMapper : EntityTypeConfiguration<LogUsuarioSistemaAcesso> {

		public LogUsuarioSistemaAcessoMapper() {
			this.ToTable("syslog_usuario_sistema_acesso");
			this.HasKey(o => o.id);
            this.HasRequired(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuario);
		}
	}
}