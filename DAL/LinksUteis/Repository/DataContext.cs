using System.Data.Entity;
using DAL.LinksUteis;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        public DbSet<LinkUtil> LinkUtil { get; set; }

		/**
		*
		*/
		private void mapperModuloLinksUteis(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new LinkUtilMapper());
		}
	}
}