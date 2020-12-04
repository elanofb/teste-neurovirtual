using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Fornecedores;

namespace DAL.Produtos {

	//
	public class EstoqueEntrada : DefaultEntity {

		public int idProdutoEstoque { get; set; }
		public virtual ProdutoEstoque ProdutoEstoque { get; set; }
		public int idFornecedor { get; set; }
		public virtual Fornecedor Fornecedor { get; set; }		
	}

	//
	internal sealed class EstoqueEntradaMapper : EntityTypeConfiguration<EstoqueEntrada> {

		public EstoqueEntradaMapper() {
			this.ToTable("tb_movimentacao_entrada");
			this.HasKey(o => o.id);

			this.HasRequired(t => t.ProdutoEstoque).WithMany().HasForeignKey(t => t.idProdutoEstoque);
            this.HasRequired(t => t.Fornecedor).WithMany().HasForeignKey(t => t.idFornecedor);
		}
	}
}