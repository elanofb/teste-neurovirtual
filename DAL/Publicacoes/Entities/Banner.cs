using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {

	public class Banner : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

	    public int? idPortal { get; set; }

	    public virtual Portal Portal { get; set; }

        public string linkBanner { get; set; }

		public string flagBlank { get; set; }

		public string posicao { get; set; }
		
		public DateTime? dtInicioExibicao  { get; set; }
		
		public DateTime? dtFimExibicao  { get; set; }

		public virtual ArquivoUpload Arquivo { get; set;}
	}

	internal sealed class BannerMapper : EntityTypeConfiguration<Banner> {

		public BannerMapper() {

			this.ToTable("tb_banner");

			this.HasKey(x => x.id);

			this.Ignore(m => m.Arquivo);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

        }
	}
}