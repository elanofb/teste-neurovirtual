using System.Data.Entity;
using DAL.Escolaridades;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<NivelEscolar> NivelEscolar { get; set; }

		/**
		*
		*/

		private void mapperModuloEscolaridades(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new NivelEscolarMapper());
		}
	}
}