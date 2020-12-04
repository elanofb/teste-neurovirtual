using System.Data.Entity;
using DAL.Relacionamentos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<OcorrenciaRelacionamentoPadrao> OcorrenciaRelacionamentoPadrao { get; set; }
		
		public DbSet<OcorrenciaRelacionamento> OcorrenciaRelacionamento { get; set; }
		
		public DbSet<OcorrenciaRelacionamentoVW> OcorrenciaRelacionamentoVW { get; set; }
		
		public DbSet<PessoaRelacionamentoVW> PessoaRelacionamentoVW { get; set; }

		//
		private void mapperModuloRelacionamento(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new OcorrenciaRelacionamentoPadraoMapper());
			
			modelBuilder.Configurations.Add(new OcorrenciaRelacionamentoMapper());
			
			modelBuilder.Configurations.Add(new OcorrenciaRelacionamentoVWMapper());
			
			modelBuilder.Configurations.Add(new PessoaRelacionamentoVWMapper());
			
		}
		
	}
	
}