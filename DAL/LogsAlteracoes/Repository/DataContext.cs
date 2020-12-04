using System.Data.Entity;
using DAL.LogsAlteracoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<LogAlteracao> LogAlteracao { get; set; }

		//
		private void mapperModuloLogsAlteracoes(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new LogAlteracaoMapper());
		}
	}
}