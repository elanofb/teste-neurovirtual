using System.Data.Entity;
using DAL.DocumentosDigitais;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        public DbSet<DocumentoDigital> DocumentoDigital { get; set; }
        public DbSet<TipoDocumentoDigital> TipoDocumentoDigital { get; set; }

        /**
		*
		*/
        private void mapperModuloDocumentosDigitais(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new DocumentoDigitalMapper());
            modelBuilder.Configurations.Add(new TipoDocumentoDigitalMapper());
        }
	}
}