using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {

	public class Video : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

	    public int? idPortal { get; set; }

	    public virtual Portal Portal { get; set; }

        public string titulo { get; set; }

		public string urlVideo { get; set; }

		public bool flagAutenticacao { get; set; }

        public bool? flagSomenteAssociado { get; set; }

	}


	internal sealed class VideoMapper : EntityTypeConfiguration<Video> {

		public VideoMapper() {

			this.ToTable("tb_video");

			this.HasKey(x => x.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

        }
	}
}