using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Notificacoes;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Notificacoes {

	//
	public class ConfiguracaoEmailUsuario  {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

        public int? idUsuario { get; set; }

		public string assinaturaEmail { get; set; }

		public string contaEmailSistema { get; set; }

		public string senhaEmailSistema { get; set; }

		public string servidorPOPEmailSistema { get; set; }

		public string flagSSLPOPEmailSistema { get; set; }

		public int portaPOPEmailSistema { get; set; }

		public string servidorSMTPEmailSistema { get; set; }

		public string flagSSLSMTPEmailSistema { get; set; }

		public int portaSMTPEmailSistema { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public UsuarioSistema UsuarioExclusao { get; set; }
	}

	//
	internal sealed class ConfiguracaoEmailUsuarioMapper : EntityTypeConfiguration<ConfiguracaoEmailUsuario> {

		public ConfiguracaoEmailUsuarioMapper() {
			
			this.ToTable("tb_configuracao_email_usuario");
			
			this.HasKey(o => o.id);

			this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
			
			this.HasOptional(x => x.UsuarioExclusao).WithMany().HasForeignKey(x => x.idUsuarioExclusao);
			
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
        }
	}
}