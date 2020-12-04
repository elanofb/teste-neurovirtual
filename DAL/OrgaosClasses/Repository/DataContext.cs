using System.Data.Entity;
using DAL.OrgaosClasses;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<OrgaoClasse> OrgaoClasse { get; set; }

		//
		private void mapperModuloOrgaosClasses(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new OrgaoClasseMapper());
		}
	}
}