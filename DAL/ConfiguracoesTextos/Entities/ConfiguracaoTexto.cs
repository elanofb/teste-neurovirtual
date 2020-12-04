using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesTextos {

	//
	public class ConfiguracaoTexto {

		public int id { get; set; }
		
		public string key { get; set; }

        public int idOrganizacao { get; set; }

        public int? idTextoCategoria { get; set; }
		
        public int? idIdioma { get; set; }
		
        public Idioma Idioma { get; set; }

        public virtual ConfiguracaoTextoCategoria ConfiguracaoTextoCategoria { get; set; }

        public string texto { get; set; }
       
		public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public virtual UsuarioSistema UsuarioSistema { get; set; }
		
		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class ConfiguracaoTextoMapper : EntityTypeConfiguration<ConfiguracaoTexto> {

		public ConfiguracaoTextoMapper() {

			this.ToTable("systb_configuracao_texto");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioAlteracao);

            this.HasOptional(x => x.ConfiguracaoTextoCategoria).WithMany().HasForeignKey(x => x.idTextoCategoria);
			
            this.HasOptional(x => x.Idioma).WithMany().HasForeignKey(x => x.idIdioma);
		}
	}
}