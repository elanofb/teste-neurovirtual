using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;

namespace DAL.Produtos {

	//
	public class ProdutoTipoAssociado {

        public int id { get; set; }

		public int idProduto { get; set; }

        public virtual Produto Produto { get; set; }

		public int idTipoAssociado { get; set; }

        public virtual TipoAssociado TipoAssociado { get; set; }

        public int idUsuarioCadastro { get; set; }		

		public DateTime dtCadastro { get; set; }		

        public int? idUsuarioExclusao { get; set; }		

		public DateTime? dtExclusao { get; set; }		
	}

	//
	internal sealed class ProdutoTipoAssociadoMapper : EntityTypeConfiguration<ProdutoTipoAssociado> {

		public ProdutoTipoAssociadoMapper() {

            this.ToTable("tb_produto_tipo_associado");

            this.HasKey(o => o.id);

            this.HasRequired(t => t.Produto).WithMany().HasForeignKey(t => t.idProduto);

            this.HasRequired(t => t.TipoAssociado).WithMany().HasForeignKey(t => t.idTipoAssociado);
		}
	}
}