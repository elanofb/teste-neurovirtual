using System.Data.Entity;
using DAL.ContasBancarias;

namespace DAL.Repository.Base {

	public partial class DataContext {
        public DbSet<ContaBancaria> ContaBancaria { get; set; }
        public DbSet<ContaBancariaMovimentacao> ContaMovimentacao { get; set; }
        public DbSet<ContaTipoOperacao> ContaTipoOperacao { get; set; }
		
		//
		private void mapperModuloContasBancarias(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new ContaBancariaMapper());
			modelBuilder.Configurations.Add(new ContaBancariaMovimentacaoMapper());
			modelBuilder.Configurations.Add(new ContaTipoOperacaoMapper());
		}
	}
}