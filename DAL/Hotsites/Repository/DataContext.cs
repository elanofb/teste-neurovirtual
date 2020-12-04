using System.Data.Entity;
using DAL.Hotsites;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Hotsite> Hotsite { get; set; }

		//
		private void mapperModuloHotsites(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new HotsiteMapper());
		}
	}
}