using System.Data.Entity;
using DAL.RelatoriosImediatos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<RelatorioImediato> RelatorioImediato { get; set; }

		private void mapperModuloRelatoriosImediatos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new RelatorioImediatoMapper());
		}
	}
}