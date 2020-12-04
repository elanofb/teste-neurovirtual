using System.Data.Entity;
using DAL.Beneficios;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Beneficio> Beneficio { get; set; }

		/**
		*
		*/

		private void mapperModuloBeneficios(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new BeneficioMapper());
		}
	}
}