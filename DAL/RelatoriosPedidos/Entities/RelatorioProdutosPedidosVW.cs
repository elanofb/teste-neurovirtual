using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RelatoriosPedidos{
    
    public class RelatorioProdutosPedidosVW {
        
        public int id { get; set; }
        
        public int? idOrganizacao { get; set; }
        
        public int? idUnidadeCliente { get; set; }
        
        public string siglaUnidadeCliente { get; set; }

        public int idPedido { get; set; }
        
        public int idStatusPedido { get; set; }
        
        public DateTime dtPedido { get; set; }

        public int idTipoProduto { get; set; }

        public string descricaoTipoProduto { get; set; }

        public int idProduto { get; set; }

        public string nome { get; set; }

        public decimal valorProduto { get; set; }
        
        public int qtdeProduto { get; set; }
        
        public decimal valorProdutosPedido { get; set; }
        
    }
    
    internal sealed class RelatorioProdutosPedidosVWMapper : EntityTypeConfiguration<RelatorioProdutosPedidosVW> {

        public RelatorioProdutosPedidosVWMapper() {

            this.ToTable("vw_produtos_pedidos");
            this.HasKey(o => o.id);

        }
    }
}