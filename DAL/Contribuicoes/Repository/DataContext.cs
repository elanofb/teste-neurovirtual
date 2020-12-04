using System.Data.Entity;
using DAL.Contribuicoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Contribuicao> Contribuicao { get; set; }

        public DbSet<ContribuicaoTabelaPreco> ContribuicaoTabelaPreco { get; set; }

		public DbSet<ContribuicaoPreco> ContribuicaoPreco { get; set; }

		public DbSet<ContribuicaoPrecoDesconto> ContribuicaoPrecoDesconto { get; set; }

		public DbSet<ContribuicaoVencimento> ContribuicaoVencimento { get; set; }

		public DbSet<TipoContribuicao> TipoContribuicao { get; set; }

        public DbSet<PeriodoContribuicao> PeriodoContribuicao { get; set; }

        public DbSet<TipoVencimento> TipoVencimento { get; set; }

        public DbSet<TipoGeracaoContribuicao> TipoGeracaoContribuicao { get; set; }

		public DbSet<ContribuicaoCobranca> ContribuicaoCobranca { get; set; }

		private void mapperModuloContribuicoes(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new ContribuicaoMapper());

            modelBuilder.Configurations.Add(new ContribuicaoTabelaPrecoMapper());

            modelBuilder.Configurations.Add(new ContribuicaoPrecoMapper());

            modelBuilder.Configurations.Add(new ContribuicaoPrecoDescontoMapper());

            modelBuilder.Configurations.Add(new ContribuicaoVencimentoMapper());

            modelBuilder.Configurations.Add(new TipoContribuicaoMapper());

            modelBuilder.Configurations.Add(new PeriodoContribuicaoMapper());

            modelBuilder.Configurations.Add(new TipoVencimentoMapper());

            modelBuilder.Configurations.Add(new TipoGeracaoContribuicaoMapper());

            modelBuilder.Configurations.Add(new ContribuicaoCobrancaMapper());
		}
	}
}