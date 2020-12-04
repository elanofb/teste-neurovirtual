using System.Data.Entity;
using DAL.Relatorios;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<ResumoAssociadoVW> ResumoAssociadoVW { get; set; }
        public DbSet<CentroCustoVW> CentroCustoVW { get; set; }
        public DbSet<AssociadoContribuicaoVW> AssociadoContribuicaoVW { get; set; }

		//
		private void mapperModuloRelatorios(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new ResumoAssociadoVWMapper());
            modelBuilder.Configurations.Add(new CentroCustoVWMapper());
            modelBuilder.Configurations.Add(new AssociadoContribuicaoVWMapper());
		}
	}
}