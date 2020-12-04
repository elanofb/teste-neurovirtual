using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;
using DAL.Organizacoes;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoRotinaAutomatica {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public bool? flagEnvioEmailCTe { get; set; }

        public bool? flagBaixarArquivosEntrada { get; set; }

        public bool? flagProcessarArquivoEntrada { get; set; }

        public bool? flagEnviarArquivoOcorrencia { get; set; }

        public bool? flagEnviarArquivoConemb { get; set; }

        public bool? flagCalcularFrete { get; set; }

        public bool? flagEmitirCTe { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set;}

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

		public bool? flagExcluido { get; set; }
	}

	//
	internal sealed class ConfiguracaoRotinaAutomaticaMapper : EntityTypeConfiguration<ConfiguracaoRotinaAutomatica> {

		public ConfiguracaoRotinaAutomaticaMapper() {
			this.ToTable("systb_configuracao_rotina_automatica ");
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
            this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
        }
	}
}