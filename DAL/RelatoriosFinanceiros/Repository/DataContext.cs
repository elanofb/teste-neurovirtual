using System.Data.Entity;
using DAL.RelatoriosFinanceiros;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<RelatorioResultadoContribuicaoVW> RelatorioResultadoContribuicaoVW { get; set; }
		
		public DbSet<ResultadoDREVW> ResultadoDREVW { get; set; }

		//
	    private void mapperModuloRelatoriosFinanceiros(DbModelBuilder modelBuilder) {
		    
	        modelBuilder.Configurations.Add(new RelatorioResultadoContribuicaoVWMapper());
		    
		    modelBuilder.Configurations.Add(new ResultadoDREVWMapper());
		    
	    }
		
	}
	
}