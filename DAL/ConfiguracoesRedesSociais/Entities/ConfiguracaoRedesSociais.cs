using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesRedesSociais {

	//
	public class ConfiguracaoRedesSociais  {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}

		public string linkInstagram { get; set; }

		public string linkFlickr { get; set; }

		public string linkTwitter { get; set; }

		public string linkFacebook { get; set; }

		public string linkYouTube { get; set; }

		public string linkGooglePlus { get; set; }

		public string linkLinkedin { get; set; }

		public bool? flagCompartilharFB { get; set; }

		public string clientIdFB { get; set; }

		public string clientSecretFB { get; set; }

		public string accessTokenUserFB { get; set; }

		public string accessTokenFanPageFB { get; set; }

		public string idFanPageFB { get; set; }

		public string nomeFanPageFB { get; set; }

		public string urlRetornoFB { get; set; }

		public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class ConfiguracaoRedesSociaisMapper : EntityTypeConfiguration<ConfiguracaoRedesSociais> {

		public ConfiguracaoRedesSociaisMapper() {

			this.ToTable("systb_configuracao_rede_social");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}