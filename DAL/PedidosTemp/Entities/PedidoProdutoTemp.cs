using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.PedidosTemp {

	//
	public class PedidoProdutoTemp {

		public int id { get; set; }

	    public int idPedido { get; set; }

        public PedidoTemp PedidoTemp { get; set; }

	    public int idProduto { get; set; }

	    public decimal valorItem { get; set; }

	    public int qtde { get; set; }

	    public decimal? peso { get; set; }

	    public string nomeProduto { get; set; }

        public string observacoes { get; set; }

        public bool? flagCalcularFrete { get; set; }
		
		public decimal? valorGanhoDiario { get; set; }
		
		public int? qtdeDiasDuracao { get; set; }
		
		public DateTime? dtFimGanhoDiario { get; set; }		

		public DateTime dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }
        
		//Construtor
		public PedidoProdutoTemp() {
			
		}
        
	}

	//
	internal sealed class PedidoProdutoTempMapper : EntityTypeConfiguration<PedidoProdutoTemp> {

		public PedidoProdutoTempMapper() {
			
			this.ToTable("temptb_pedido_produto");
			
			this.HasKey(o => o.id);
		
            this.HasRequired(x => x.PedidoTemp).WithMany(x => x.listaProdutos).HasForeignKey(x => x.idPedido);
            	
		}

	}

}