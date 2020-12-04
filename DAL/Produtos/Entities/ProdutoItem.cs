using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Produtos {

	//
	public class ProdutoItem {

        public int id { get; set; }

        public string descricao { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidadeMedida { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }

        public string observacoes { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

    }

	//
	internal sealed class ProdutoItemMapper : EntityTypeConfiguration<ProdutoItem> {

		public ProdutoItemMapper() {
			this.ToTable("tb_produto_item");
			this.HasKey(o => o.id);

		    this.HasRequired(x => x.UnidadeMedida).WithMany().HasForeignKey(x => x.idUnidadeMedida);
		}
	}
}