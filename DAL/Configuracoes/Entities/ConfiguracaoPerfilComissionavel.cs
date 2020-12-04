using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Permissao;

namespace DAL.Configuracoes {
	
	public class ConfiguracaoPerfilComissionavel {

		public int id { get; set; }

        public int idConfiguracaoComissao { get; set; }

        public virtual ConfiguracaoComissao ConfiguracaoComissao { get; set; }

        public int idPerfilAcesso { get; set; }

        public virtual PerfilAcesso PerfilAcesso { get; set; }
        
        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
    }

	
	internal sealed class ConfiguracaoPerfilComissionavelMapper : EntityTypeConfiguration<ConfiguracaoPerfilComissionavel> {

		public ConfiguracaoPerfilComissionavelMapper() {
			this.ToTable("systb_configuracao_perfil_comissionavel");

			this.HasKey(x => x.id);

		    this.HasRequired(x => x.ConfiguracaoComissao).WithMany(x => x.listaPerfisComissionaveis).HasForeignKey(x => x.idConfiguracaoComissao);
		    this.HasRequired(x => x.PerfilAcesso).WithMany().HasForeignKey(x => x.idPerfilAcesso);
		}
	}
}