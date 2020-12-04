using System.Data.Entity;
using DAL.AssociadosContribuicoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<AssociadoContribuicao> AssociadoContribuicao { get; set; }

		public DbSet<AssociadoContribuicaoEmailCobranca> AssociadoContribuicaoEmailCobranca { get; set; }

		public DbSet<AssociadoContribuicaoFilaGeracao> AssociadoContribuicaoFilaGeracao { get; set; }

		public DbSet<AssociadoContribuicaoBoletoGeracao> AssociadoContribuicaoBoletoGeracao { get; set; }

		public DbSet<AssociadoContribuicaoBoleto> AssociadoContribuicaoBoleto { get; set; }

        public DbSet<AssociadoContribuicaoResumoVW> AssociadoContribuicaoResumoVW { get; set; }

        public DbSet<AssociadoContribuicaoOrdenada> AssociadoContribuicaoOrdenada { get; set; }
        
		//
		private void mapperModuloAssociadosContribuicoes(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new AssociadoContribuicaoMapper());

            modelBuilder.Configurations.Add(new AssociadoContribuicaoEmailCobrancaMapper());

            modelBuilder.Configurations.Add(new AssociadoContribuicaoFilaGeracaoMapper());

            modelBuilder.Configurations.Add(new AssociadoContribuicaoBoletoGeracaoMapper());

            modelBuilder.Configurations.Add(new AssociadoContribuicaoBoletoMapper());

            modelBuilder.Configurations.Add(new AssociadoContribuicaoResumoVWMapper());

            modelBuilder.Configurations.Add(new AssociadoContribuicaoOrdenadaMapper());


		}
	}
}