using System.Data.Entity;
using DAL.Portais;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Portal> Portal { get; set; }
		
		private void mapperModuloPortais(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new PortalMapper());
        }
	}
}