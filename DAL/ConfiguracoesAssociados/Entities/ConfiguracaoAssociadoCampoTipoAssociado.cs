using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Permissao;

namespace DAL.ConfiguracoesAssociados {

	//
    public class ConfiguracaoAssociadoCampoTipoAssociado {

        public int id { get; set; }
        
        public int idConfiguracaoAssociadoCampo { get; set; }

        public ConfiguracaoAssociadoCampo AssociadoCampo { get; set; }

        public int idTipoAssociado { get; set; }

        public TipoAssociado TipoAssociado { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
    }

    //
	internal sealed class ConfiguracaoAssociadoCampoTipoAssociadoMapper : EntityTypeConfiguration<ConfiguracaoAssociadoCampoTipoAssociado> {

		public ConfiguracaoAssociadoCampoTipoAssociadoMapper() {

			this.ToTable("systb_configuracao_associado_campo_tipo_associado");

            this.HasKey(o => o.id);

		    this.HasRequired(x => x.AssociadoCampo).WithMany(x => x.listaTipoAssociado).HasForeignKey(x => x.idConfiguracaoAssociadoCampo);

		    this.HasRequired(x => x.TipoAssociado).WithMany().HasForeignKey(x => x.idTipoAssociado);

		    this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
		}
	}
}