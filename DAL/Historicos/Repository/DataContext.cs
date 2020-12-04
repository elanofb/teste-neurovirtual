using System.Data.Entity;
using DAL.DadosBancarios;
using DAL.Historicos;

namespace DAL.Repository.Base {

	public partial class DataContext {

		public DbSet<HistoricoAtualizacao> HistoricoAtualizacao { get; set; }
		
		//
		private void mapperModuloHistoricos(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new HistoricoAtualizacaoMapper());

		}
		
	}
}