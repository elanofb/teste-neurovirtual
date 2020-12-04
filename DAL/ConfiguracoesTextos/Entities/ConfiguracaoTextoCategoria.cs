using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.ConfiguracoesTextos {

	//
	public class ConfiguracaoTextoCategoria {

		public int id { get; set; }

        public string descricao { get; set; }
       
		public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual UsuarioSistema UsuarioSistema { get; set; }
	}

	//
	internal sealed class ConfiguracaoTextoCategoriaMapper : EntityTypeConfiguration<ConfiguracaoTextoCategoria> {

		public ConfiguracaoTextoCategoriaMapper() {

			this.ToTable("systb_configuracao_texto_categoria");

            this.HasKey(o => o.id);

		    this.HasRequired(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
		}
	}
}