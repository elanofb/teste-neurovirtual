using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAssociados {

	//
	public class ConfiguracaoAssociadoCampoOpcao {

		public int id { get; set; }

        public int idConfiguracaoAssociadoCampo { get; set; }

        public virtual ConfiguracaoAssociadoCampo ConfiguracaoAssociadoCampo { get; set; }

        public string value { get; set; }

        public string texto { get; set; }

		public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class ConfiguracaoAssociadoCampoOpcaoMapper : EntityTypeConfiguration<ConfiguracaoAssociadoCampoOpcao> {

		public ConfiguracaoAssociadoCampoOpcaoMapper() {

			this.ToTable("systb_configuracao_associado_campo_opcao");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasRequired(x => x.ConfiguracaoAssociadoCampo).WithMany(x => x.listaCampoOpcoes).HasForeignKey(x => x.idConfiguracaoAssociadoCampo);
		}
	}
}