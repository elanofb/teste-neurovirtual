using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Produtos;

namespace DAL.Pedidos {

	//
	public class PedidoProduto {

		public int id { get; set; }

		public int idPedido { get; set; }

		public Pedido Pedido { get; set; }

		public int idProduto { get; set; }

		public Produto Produto { get; set; }

		public decimal? valorItem { get; set; }

        public decimal? valorOriginal { get; set; }

        public decimal? valorDesconto { get; set; }

		public int? qtde { get; set; }

		public decimal? peso { get; set; }

		public string nomeProduto { get; set; }

	    public bool? flagCalcularFrete { get; set; }
		
		public decimal? valorGanhoDiario { get; set; }
		
		public decimal? qtdePontosPlanoCarreira { get; set; }
		
		public int? qtdeDiasDuracao { get; set; }
		
		public DateTime? dtFimGanhoDiario { get; set; }
		
		public DateTime? dtUltimoPagamento { get; set; } 

        public string observacoes { get; set; }

        public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }

		public decimal getValorTotalItem() {
			return Decimal.Multiply(this.valorItem.toDecimal(), this.qtde.toInt());
		}
	}

	/**
	*
	*/

	internal sealed class PedidoProdutoMapper : EntityTypeConfiguration<PedidoProduto> {

		public PedidoProdutoMapper() {
			this.ToTable("tb_pedido_produto");
			this.HasKey(o => o.id);
			this.HasRequired(t => t.Pedido).WithMany(p => p.listaProdutos).HasForeignKey(t => t.idPedido);
			this.HasRequired(t => t.Produto).WithMany().HasForeignKey(t => t.idProduto);
		}
	}
}