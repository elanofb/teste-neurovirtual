using System.Data.Entity;
using DAL.Fornecedores;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Fornecedor> Fornecedor { get; set; }

		/**
		*
		*/

		private void mapperModuloFornecedores(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new FornecedorMapper());
		}
	}
}