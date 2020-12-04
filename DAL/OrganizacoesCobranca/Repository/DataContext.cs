using System.Data.Entity;
using DAL.OrganizacoesCobranca;

namespace DAL.Repository.Base {

	public partial class DataContext {

		public DbSet<OrganizacaoCobranca> OrganizacaoCobranca { get; set; }
		
		//
		private void mapperModuloOrganizacoesCobrancas(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new OrganizacaoCobrancaMapper());

		}
	}
}