using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.AreasAtuacao {

	//
	public class AreaAtuacao : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

	}

	//
	internal sealed class AreaAtuacaoMapper : EntityTypeConfiguration<AreaAtuacao> {

		public AreaAtuacaoMapper() {

			this.ToTable("tb_area_atuacao");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}