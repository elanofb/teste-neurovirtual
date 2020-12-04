using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoTipoCampo {

		public int id { get; set; }

        public string tipo { get; set; }

		public string descricao { get; set; }

		public bool? flagOpcoes { get; set; }

		public bool? ativo { get; set; }

		public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioSistema { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class ConfiguracaoTipoCampoMapper : EntityTypeConfiguration<ConfiguracaoTipoCampo> {

		public ConfiguracaoTipoCampoMapper() {

			this.ToTable("datatb_configuracao_tipo_campo");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

		}
	}
}