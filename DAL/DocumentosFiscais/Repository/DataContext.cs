using System.Data.Entity;
using DAL.DocumentosFiscais;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
		public DbSet<UnidadeConfiguracao> UnidadeConfiguracao { get; set; }
		public DbSet<UnidadeNumeracaoDocumento> UnidadeNumeracaoDocumento { get; set; }

        /**
		*
		*/
        private void mapperModuloDocumentosFiscais(DbModelBuilder modelBuilder) {
	        modelBuilder.Configurations.Add(new UnidadeConfiguracaoMapper());
	        modelBuilder.Configurations.Add(new UnidadeNumeracaoDocumentoMapper());
        }
	}
}