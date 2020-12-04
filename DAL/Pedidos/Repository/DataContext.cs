using System.Data.Entity;
using DAL.Pedidos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Pedido> Pedido { get; set; }

		public DbSet<PedidoEntrega> PedidoEntrega { get; set; }

		public DbSet<PedidoProduto> PedidoProduto { get; set; }

		public DbSet<TipoOcorrenciaPedido> TipoOcorrenciaPedido { get; set; }

		public DbSet<PedidoHistorico> PedidoHistorico { get; set; }

		public DbSet<StatusPedido> StatusPedido { get; set; }
		
		public DbSet<PedidoProdutoRendimento> PedidoProdutoRendimento { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		private void mapperModuloPedidos(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new PedidoMapper());
			
			modelBuilder.Configurations.Add(new PedidoEntregaMapper());
			
			modelBuilder.Configurations.Add(new PedidoProdutoMapper());
			
			modelBuilder.Configurations.Add(new TipoOcorrenciaPedidoMapper());
			
			modelBuilder.Configurations.Add(new PedidoHistoricoMapper());
			
			modelBuilder.Configurations.Add(new StatusPedidoMapper());
			
			modelBuilder.Configurations.Add(new PedidoProdutoRendimentoMapper());
		}
	}
}