using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Financeiro {

	//
	public class DetalheTipoCategoriaTitulo : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public int idTipoCategoria { get; set; }

		public virtual TipoCategoriaTitulo TipoCategoria { get; set; }

	}

	//
	internal sealed class DetalheTipoCategoriaTituloMapper : EntityTypeConfiguration<DetalheTipoCategoriaTitulo> {

		public DetalheTipoCategoriaTituloMapper() {

			this.ToTable("tb_financeiro_categoria_tipo_detalhe");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(x => x.TipoCategoria).WithMany().HasForeignKey(x => x.idTipoCategoria);

		}
	}
}