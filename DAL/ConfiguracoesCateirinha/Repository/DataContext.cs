using System.Data.Entity;
using DAL.ConfiguracoesCateirinha;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        

        public DbSet<ConfiguracaoCarteirinha> ConfiguracaoCarteirinha { get; set; }

		//
		private void mapperModuloConfiguracoesCarteirinha(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new ConfiguracaoCarteirinhaMapper());
		
		}
	}
}