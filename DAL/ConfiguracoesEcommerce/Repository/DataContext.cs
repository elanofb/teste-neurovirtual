using System.Data.Entity;
using DAL.Configuracoes;
using DAL.ConfiguracoesEcommerce;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        
		public DbSet<ConfiguracaoEcommerce> ConfiguracaoEcommerce { get; set; }

        //
        private void mapperModuloConfiguracoesEcommerce(DbModelBuilder modelBuilder) {
            
			modelBuilder.Configurations.Add(new ConfiguracaoEcommerceMapper());

        }

	}

}