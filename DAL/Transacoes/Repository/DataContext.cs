using System.Data.Entity;
using DAL.Transacoes;


namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoTransacao> TipoTransacao { get; set; }
		
		public DbSet<Movimento> Movimento { get; set; }
		
		public DbSet<MovimentoResumoVW> MovimentoResumoVW { get; set; }
		
		public DbSet<TotalMovimentoVW> TotalMovimentoVW { get; set; }
		
		public DbSet<ConferenciaSaldoVW> ConferenciaSaldoVW { get; set; }

		//
		private void mapperModuloTransacoes(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new TipoTransacaoMapper());
			
			modelBuilder.Configurations.Add(new MovimentoMapper());
			
			modelBuilder.Configurations.Add(new MovimentoResumoVWMapper());
			
			modelBuilder.Configurations.Add(new TotalMovimentoVWMapper());
			
			modelBuilder.Configurations.Add(new ConferenciaSaldoVWMapper());
		}
	}
}