using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoSistema {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao {get; set;}

        public string codigoOrganizacao { get; set; }

		public string siglaOrganizacao { get; set; }

		public string tituloSistema { get; set; }

		public string nomeEmpresaResumo { get; set; }

		public string nomeEmpresaCompleto { get; set; }

        public string dominios { get; set; }

        public string htmlLogoTopo { get; set; }

        public string htmlLogoTopoMini { get; set; }

		public string temaInterface { get; set; }

        public string rotaCustomizadaLogin { get; set; }

        public bool? flagBgLoginCustomizado { get; set;}

        public string tituloCaixaLogin { get; set;}

		public string cssCustomizadoLogin { get; set; }

		public string cssCustomizado { get; set; }

        public string apiChaveAcesso { get; set; }

		public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public UsuarioSistema UsuarioSistema { get; set; }

        public bool? flagExcluido { get; set;}
	}

	//
	internal sealed class ConfiguracaoSistemaMapper : EntityTypeConfiguration<ConfiguracaoSistema> {

		public ConfiguracaoSistemaMapper() {

			this.ToTable("systb_configuracao_sistema");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		}
	}
}