using System.Data.Entity;
using DAL.RelatoriosImediatos;
using DAL.RelatoriosPedidos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<RelatorioProdutosPedidosVW> RelatorioProdutosPedidosVW { get; set; }

		private void mapperModuloRelatoriosPedidos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new RelatorioProdutosPedidosVWMapper());
		}
	}
}