using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Configuracoes {
	
	public class ConfiguracaoComissao {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

        public bool? flagHabilitar { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual List<ConfiguracaoPerfilComissionavel> listaPerfisComissionaveis { get; set; } 
    }

	
	internal sealed class ConfiguracaoComissaoMapper : EntityTypeConfiguration<ConfiguracaoComissao> {

		public ConfiguracaoComissaoMapper() {
			this.ToTable("systb_configuracao_comissao");
			this.HasKey(x => x.id);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		    this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
		}
	}
}