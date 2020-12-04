using System.Data.Entity;
using DAL.ConfiguracoesAssociados;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        
        public DbSet<ConfiguracaoAssociadoPF> ConfiguracaoAssociadoPF { get; set; }

        public DbSet<ConfiguracaoAssociadoPJ> ConfiguracaoAssociadoPJ { get; set; }

        public DbSet<ConfiguracaoAssociadoCampo> ConfiguracaoAssociadoCampo { get; set; }

        public DbSet<ConfiguracaoAssociadoCampoGrupo> ConfiguracaoAssociadoCampoGrupo { get; set; }

        public DbSet<ConfiguracaoAssociadoCampoOpcao> ConfiguracaoAssociadoCampoOpcao { get; set; }

        public DbSet<ConfiguracaoAssociadoCampoPropriedade> ConfiguracaoAssociadoCampoPropriedade { get; set; }
        
        public DbSet<ConfiguracaoAssociadoCampoTipoAssociado> ConfiguracaoAssociadoCampoTipoAssociado { get; set; }

        public DbSet<FuncaoFiltro> FuncaoFiltro { get; set; }

		//
		private void mapperModuloConfiguracoesAssociados(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new ConfiguracaoAssociadoPFMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoAssociadoPJMapper());
		
            modelBuilder.Configurations.Add(new ConfiguracaoAssociadoCampoMapper());
		
            modelBuilder.Configurations.Add(new ConfiguracaoAssociadoCampoGrupoMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoAssociadoCampoOpcaoMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoAssociadoCampoPropriedadeMapper());

		    modelBuilder.Configurations.Add(new ConfiguracaoAssociadoCampoTipoAssociadoMapper());

            modelBuilder.Configurations.Add(new FuncaoFiltroMapper());
		}
	}
}