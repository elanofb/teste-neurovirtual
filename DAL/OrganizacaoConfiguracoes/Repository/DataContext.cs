using System.Data.Entity;
using DAL.OrganizacaoConfiguracoes;
using DAL.Organizacoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<OrganizacaoDadosAssociado> OrganizacaoDadosAssociado { get; set; }
		
        //
        private void mapperModuloOrganizacaoConfiguracoes(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new OrganizacaoDadosAssociadoMapper());

        }
		
	}
	
}