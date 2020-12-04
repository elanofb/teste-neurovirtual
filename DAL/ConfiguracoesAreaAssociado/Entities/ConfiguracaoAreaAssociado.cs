using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAreaAssociado {

	//
	public class ConfiguracaoAreaAssociado {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}
        
        public bool? flagAreaRestrita { get; set; }

	    public bool? flagInterfaceIframe { get; set; }

        public bool? flagLoginAssociado { get; set; }

        public bool? flagLoginNaoAssociado { get; set; }

	    public bool? flagCadastroAssociado { get; set; }

	    public bool? flagCadastroNaoAssociado { get; set; }

	    public bool? flagCaptarMailing { get; set; }

        public string cssCustomizado { get; set; }

        public string htmlRodape { get; set; }

        public string htmlTopo { get; set; }

        public string htmlHome { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class ConfiguracaoAreaAssociadoMapper : EntityTypeConfiguration<ConfiguracaoAreaAssociado> {

		public ConfiguracaoAreaAssociadoMapper() {

			this.ToTable("systb_configuracao_area_associado");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		}
	}
}