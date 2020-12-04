using System.Data.Entity;
using DAL.Compras;

namespace DAL.Repository.Base {

	public partial class DataContext {

		public DbSet<CarrinhoItem> CarrinhoItem { get; set; }

		public DbSet<CarrinhoResumo> CarrinhoResumo { get; set; }

        public DbSet<CarrinhoItemProdutoVW> CarrinhoItemProdutoVW { get; set; }

		//
		private void mapperModuloCompras(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new CarrinhoItemMapper());

			modelBuilder.Configurations.Add(new CarrinhoResumoMapper());

			modelBuilder.Configurations.Add(new CarrinhoItemProdutoVWMapper());
		}
	}
}