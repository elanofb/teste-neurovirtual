using System.Data.Entity;
using DAL.Configuracoes;
using DAL.ConfiguracoesEtiquetas;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<ConfiguracaoEtiqueta> ConfiguracaoEtiqueta { get; set; }

        //
        private void mapperModuloConfiguracoesEtiquetas(DbModelBuilder modelBuilder) {
            
			modelBuilder.Configurations.Add(new ConfiguracaoEtiquetaMapper());
			
        }
	}
}