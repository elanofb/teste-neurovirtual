using System.Data.Entity;
using DAL.Erros;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<LogErro> LogErro { get; set; }

		/**
		*
		*/

		private void mapperModuloErros(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new LogErroMapper());
		}
	}
}