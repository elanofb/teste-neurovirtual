using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;

namespace DAL.AreasAtuacao {

	//
	public class AreaAtuacaoTipoAssociado : Geral {

		public int idTipoAssociado { get; set; }

		public virtual TipoAssociado TipoAssociado { get; set; }

		public int idAreaAtuacao { get; set; }

		public virtual AreaAtuacao AreaAtuacao { get; set; }

	}

	//
	internal sealed class AreaAtuacaoTipoAssociadoMapper : EntityTypeConfiguration<AreaAtuacaoTipoAssociado> {

		public AreaAtuacaoTipoAssociadoMapper() {
			this.ToTable("tb_area_atuacao_tipo_associado");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.TipoAssociado).WithMany().HasForeignKey(o => o.idTipoAssociado);
			this.HasRequired(o => o.AreaAtuacao).WithMany().HasForeignKey(o => o.idAreaAtuacao);
        }
	}
}