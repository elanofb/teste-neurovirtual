using System.Data.Entity;
using DAL.Beneficiarios;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Beneficiario> Beneficiario { get; set; }		

		/**
		*
		*/

		private void mapperModuloBeneficiarios(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new BeneficiarioMapper());
			
		}
	}
}