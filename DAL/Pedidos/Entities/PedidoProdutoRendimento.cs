using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Pedidos {

    public class PedidoProdutoRendimento {
        
        public int       id               { get; set; }
        
        public int       idPedido         { get; set; }
        
        public string    nomePessoa       { get; set; }
        
        public int?      nroMembro        { get; set; }
        
        public int       idMembro         { get; set; }
        
        public int idPessoa { get; set; }
        
        public string    nomeProduto      { get; set; }
        
        public decimal? valorProduto     { get; set; }
        
        public DateTime? dtQuitacao       { get; set; }
        
        public decimal? valorGanhoDiario { get; set; }
        
        public DateTime? dtFimGanhoDiario { get; set; }
        
        public DateTime? dtUltimoPagamento { get; set; }
    }

    internal sealed class PedidoProdutoRendimentoMapper : EntityTypeConfiguration<PedidoProdutoRendimento> {

        public PedidoProdutoRendimentoMapper() {
            
            this.ToTable("vw_pedido_produto_rendimento");
            
            this.HasKey(o => o.id);
        }
    }    
}
