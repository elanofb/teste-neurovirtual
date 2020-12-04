using System.Data.Entity;
using DAL.ConfiguracoesAreaAssociado;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        

        public DbSet<ConfiguracaoAreaAssociado> ConfiguracaoAreaAssociado { get; set; }

        public DbSet<AreaAssociadoGrupo> AreaAssociadoGrupo { get; set; }

        public DbSet<AreaAssociadoRecurso> AreaAssociadoRecurso { get; set; }

	    public DbSet<AreaAssociadoGrupoDefault> AreaAssociadoGrupoDefault { get; set; }

	    public DbSet<AreaAssociadoRecursoDefault> AreaAssociadoRecursoDefault { get; set; }

		//
		private void mapperModuloConfiguracoesAreaAssociado(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new ConfiguracaoAreaAssociadoMapper());

            modelBuilder.Configurations.Add(new AreaAssociadoGrupoMapper());

            modelBuilder.Configurations.Add(new AreaAssociadoRecursoMapper());

		    modelBuilder.Configurations.Add(new AreaAssociadoGrupoDefaultMapper());

		    modelBuilder.Configurations.Add(new AreaAssociadoRecursoDefaultMapper());
		
		}
	}
}