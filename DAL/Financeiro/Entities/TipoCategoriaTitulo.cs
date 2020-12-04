using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Financeiro {

	//
	public class TipoCategoriaTitulo : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public int idCategoria { get; set; }

		public virtual CategoriaTitulo Categoria { get; set; }

	}

	//
	internal sealed class TipoCategoriaTituloMapper : EntityTypeConfiguration<TipoCategoriaTitulo> {

		public TipoCategoriaTituloMapper() {

			this.ToTable("tb_financeiro_tipo_categoria");

            this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(x => x.Categoria).WithMany().HasForeignKey(x => x.idCategoria);

		}
	}
}