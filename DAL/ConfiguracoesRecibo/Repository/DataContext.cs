using System.Data.Entity;
using DAL.ConfiguracoesRecibo;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        

        public DbSet<ConfiguracaoRecibo> ConfiguracaoRecibo { get; set; }

		//
		private void mapperModuloConfiguracoesRecibo(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new ConfiguracaoReciboMapper());
		
		}
	}
}