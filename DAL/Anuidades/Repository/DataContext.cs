using System.Data.Entity;
using DAL.Anuidades;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Anuidade> Anuidade { get; set; }

		public DbSet<ExportacaoAnuidadePDF> ExportacaoAnuidadePDF { get; set; }

		/**
		*
		*/

		private void mapperModuloAnuidades(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new AnuidadeMapper());
			modelBuilder.Configurations.Add(new ExportacaoAnuidadePDFMapper());
		}
	}
}