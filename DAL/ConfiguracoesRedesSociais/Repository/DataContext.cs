using System.Data.Entity;
using DAL.Configuracoes;
using DAL.ConfiguracoesRedesSociais;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<ConfiguracaoRedesSociais> ConfiguracaoRedesSociais { get; set; }

        //
        private void mapperModuloConfiguracoesRedesSociais(DbModelBuilder modelBuilder) {
            
			modelBuilder.Configurations.Add(new ConfiguracaoRedesSociaisMapper());
			
        }
	}
}