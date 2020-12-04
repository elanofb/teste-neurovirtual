using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Fornecedores;

namespace DAL.Produtos {

	//
	public class ProdutoEstoque : DefaultEntity {

		public int idProduto { get; set; }
		public virtual Produto Produto { get; set; }
		public int qtdMovimentada { get; set; }		
		public DateTime? dtMovimentacao { get; set; }		
		public int saldoAnterior { get; set; }		
		public string descricao { get; set; }		
	}

	//
	internal sealed class ProdutoEstoqueMapper : EntityTypeConfiguration<ProdutoEstoque> {

		public ProdutoEstoqueMapper() {
			this.ToTable("tb_produto_estoque");
			this.HasKey(o => o.id);
			this.HasRequired(t => t.Produto).WithMany().HasForeignKey(t => t.idProduto);
		}
	}
}