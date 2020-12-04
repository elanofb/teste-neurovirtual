using System.Data.Entity;
using DAL.PedidosTemp;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<PedidoTemp> PedidoTemp { get; set; }

		public DbSet<PedidoProdutoTemp> PedidoProdutoTemp { get; set; }

		//
		private void mapperModuloPedidosTemp(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new PedidoTempMapper());

			modelBuilder.Configurations.Add(new PedidoProdutoTempMapper());

		}
	}
}