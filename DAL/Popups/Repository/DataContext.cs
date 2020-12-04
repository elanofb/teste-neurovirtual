using System.Data.Entity;
using DAL.Popups;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
		public DbSet<HomePopup> HomePopup { get; set; }

		/**
		*
		*/
		private void mapperModuloPopups(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new HomePopupMapper());
		}
	}
}