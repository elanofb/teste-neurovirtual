using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesRecibo {

	//
	public class ConfiguracaoRecibo {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}
        
        public bool? flagHabilitado { get; set; }

        public string htmlRecibo { get; set; }

		public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class ConfiguracaoReciboMapper : EntityTypeConfiguration<ConfiguracaoRecibo> {

		public ConfiguracaoReciboMapper() {

			this.ToTable("systb_configuracao_recibo");

            this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            
		}
	}
}