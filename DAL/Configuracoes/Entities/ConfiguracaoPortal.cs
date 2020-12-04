using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoPortal {

		public int id { get; set; }

        public string flagTipoPessoa { get; set; }

		public bool? flagSolicitarMailing { get; set; }

		public bool? flagSolicitarAbrangencia { get; set; }

		public bool? flagSolicitarAreaAtuacao { get; set; }

		public bool? flagSolicitarRepresentante { get; set; }

        public bool? flagCadastroAssociado { get; set; }

        public bool? flagNaoAssociado { get; set; }

        public bool? flagAcessoNaoAssociado { get; set; }

        public bool? flagExibirTipoNaoAssociado { get; set; }

        public string htmlInstrucoesNovoCadastroNaoAssociado { get; set; }

        public string htmlInstrucoesEdicaoCadastroNaoAssociado { get; set; }
        
        public string cssCustom { get; set; }

        public string googleMaps { get; set; }

		public string googleAnalytics { get; set; }

		public string htmlFooter { get; set; }

		public DateTime dtCadastro { get; set; }

		public int idUsuarioCadastro { get; set; }

		public bool? flagExcluido { get; set; }

	}

	//
	internal sealed class ConfiguracaoPortalMapper : EntityTypeConfiguration<ConfiguracaoPortal> {

		public ConfiguracaoPortalMapper() {
			this.ToTable("systb_configuracao_portal");
			this.HasKey(o => o.id);
		}
	}
}