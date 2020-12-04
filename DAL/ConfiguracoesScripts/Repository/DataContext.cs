using System.Data.Entity;
using DAL.ConfiguracoesScripts;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        

        public DbSet<ConfiguracaoScripts> ConfiguracaoScripts { get; set; }
        
		//
		private void mapperModuloConfiguracoesScripts(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new ConfiguracaoScriptsMapper());
		
		}
	}
}