using System.Data.Entity;
using DAL.Cargos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Cargo> Cargo { get; set; }

		/**
		*
		*/

		private void mapperModuloCargos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new CargoMapper());
		}
	}
}