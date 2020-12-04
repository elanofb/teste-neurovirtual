using System.Data.Entity;
using DAL.Permissao;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<UsuarioSistema> UsuarioSistema { get; set; }

		public DbSet<PerfilAcesso> PerfilAcesso { get; set; }

		public DbSet<AcessoPermissao> AcessoPermissao { get; set; }

		public DbSet<AcessoRecurso> AcessoRecurso { get; set; }

		public DbSet<AcessoRecursoAcao> AcessoRecursoAcao { get; set; }

		public DbSet<AcessoRecursoGrupo> AcessoRecursoGrupo { get; set; }

		//Views
		public DbSet<UsuarioSistemaVW> UsuarioSistemaVW { get; set; }

		public DbSet<RecursoSistemaVW> RecursoSistemaVW { get; set; }

		public DbSet<RecursoPermissaoVW> RecursoPermissaoVW { get; set; }

        public DbSet<UsuarioSistemaLogadoVW> UsuarioSistemaLogadoVW { get; set; }

        /// <summary>
        /// Configuracao de mapeamentos do modulo de permissoes
        /// </summary>
        /// <param name="modelBuilder"></param>
		private void mapperModuloPermissao(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new UsuarioSistemaMapper());

            modelBuilder.Configurations.Add(new PerfilAcessoMapper());

			modelBuilder.Configurations.Add(new AcessoPermissaoMapper());
			modelBuilder.Configurations.Add(new AcessoRecursoMapper());
			modelBuilder.Configurations.Add(new AcessoRecursoAcaoMapper());
			modelBuilder.Configurations.Add(new AcessoRecursoGrupoMapper());

			modelBuilder.Configurations.Add(new UsuarioSistemaVWMapper());
			modelBuilder.Configurations.Add(new RecursoSistemaVWMapper());
			modelBuilder.Configurations.Add(new RecursoPermissaoVWMapper());
            modelBuilder.Configurations.Add(new UsuarioSistemaLogadoVWMapper());
		}
	}
}