using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.ConfiguracoesTextos {

	//
	public class ConfiguracaoLabel {

		public int id { get; set; }
		
		public string key { get; set; }

        public int? idOrganizacao { get; set; }
		
		public int? idIdioma { get; set; }
		
		public Idioma Idioma { get; set; }

        public string label { get; set; }
       
		public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public virtual UsuarioSistema UsuarioSistema { get; set; }
		
		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class ConfiguracaoLabelMapper : EntityTypeConfiguration<ConfiguracaoLabel> {

		public ConfiguracaoLabelMapper() {

			this.ToTable("systb_configuracao_label");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioAlteracao);
		    this.HasOptional(x => x.Idioma).WithMany().HasForeignKey(x => x.idIdioma);
		}
	}
}