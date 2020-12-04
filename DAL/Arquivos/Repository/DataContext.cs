using System.Data.Entity;
using DAL.Arquivos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<ArquivoUpload> ArquivoUpload { get; set; }
		public DbSet<ArquivoAssociadoVW> ArquivoAssociadoVW { get; set; }

        /**
		*
		*/

        private void mapperModuloArquivoUpload(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new ArquivoUploadMapper());
			modelBuilder.Configurations.Add(new ArquivoAssociadoVWMapper());
        }
	}
}