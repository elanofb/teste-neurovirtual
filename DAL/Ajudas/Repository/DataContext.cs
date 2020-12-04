using System.Data.Entity;
using DAL.Ajudas;
using DAL.Anuidades;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<AjudaCategoria> AjudaCategoria { get; set; }

		public DbSet<AjudaModulo> AjudaModulo { get; set; }

		/**
		*
		*/

		private void mapperModuloAjudas(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new AjudaCategoriaMapper());
			modelBuilder.Configurations.Add(new AjudaModuloMapper());
		}
	}
}