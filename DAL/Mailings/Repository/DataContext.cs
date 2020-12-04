using System.Data.Entity;
using DAL.Mailings;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Mailing> Mailing { get; set; }
		public DbSet<TipoMailing> TipoMailing { get; set; }

		/**
		*
		*/

		private void mapperModuloMailings(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new MailingMapper());
			modelBuilder.Configurations.Add(new TipoMailingMapper());
		}
	}
}