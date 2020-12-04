using System.Data.Entity;
using DAL.Organizacoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Organizacao> Organizacao { get; set; }

		public DbSet<UsuarioOrganizacao> UsuarioOrganizacao { get; set; }

		public DbSet<StatusOrganizacao> StatusOrganizacao { get; set; }

	    public DbSet<AcessoRecursoGrupoOrganizacao> AcessoRecursoGrupoOrganizacao { get; set; }

        //
        private void mapperModuloOrganizacoes(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new OrganizacaoMapper());

			modelBuilder.Configurations.Add(new UsuarioOrganizacaoMapper());

			modelBuilder.Configurations.Add(new StatusOrganizacaoMapper());

            modelBuilder.Configurations.Add(new AcessoRecursoGrupoOrganizacaoMapper());

        }
	}
}