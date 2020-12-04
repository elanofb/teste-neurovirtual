using System.Data.Entity;
using DAL.Planos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
		
		public DbSet<Plano> Plano { get; set; }
		public DbSet<PlanoPeriodo> PlanoPeriodo { get; set; }
		public DbSet<StatusPlanoContratacao> StatusPlanoContratacao { get; set; }
		public DbSet<PlanoContratacao> PlanoContratacao { get; set; }
		public DbSet<PlanoCarreira> PlanoCarreira { get; set; }
		
		/**
		*
		*/
		
		private void mapperModuloPlanos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new PlanoMapper());
			modelBuilder.Configurations.Add(new PlanoPeriodoMapper());
			modelBuilder.Configurations.Add(new PlanoContratacaoMapper());
			modelBuilder.Configurations.Add(new StatusPlanoContratacaoMapper());
			modelBuilder.Configurations.Add(new PlanoCarreiraMapper());
		}
	}
}