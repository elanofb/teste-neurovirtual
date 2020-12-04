using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Mailings {

	public class Mailing : DefaultEntity {

	    public int? idOrganizacao { get; set; }

	    public Organizacao Organizacao { get; set; }

		public string ip { get; set; }

		public string nome { get; set; }

		public string email { get; set; }

        public int? idAssociado { get; set; }

		public Associado Associado { get; set; }

		public int? idTipoMailing { get; set; }

		public TipoMailing TipoMailing { get; set; }

	}

	internal sealed class MailingMapper : EntityTypeConfiguration<Mailing> {

		public MailingMapper() {

			this.ToTable("tb_mailing");

			this.HasKey(x => x.id);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasOptional(o => o.TipoMailing).WithMany().HasForeignKey(o => o.idTipoMailing);

            this.HasOptional(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);

		}
	}
}