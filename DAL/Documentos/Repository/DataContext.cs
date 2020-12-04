using System.Data.Entity;
using DAL.Documentos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoDocumento> TipoDocumento { get; set; }

		public DbSet<CategoriaDocumento> CategoriaDocumento { get; set; }

		//
		private void mapperModuloDocumentos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new TipoDocumentoMapper());
			modelBuilder.Configurations.Add(new CategoriaDocumentoMapper());
		}
	}
}