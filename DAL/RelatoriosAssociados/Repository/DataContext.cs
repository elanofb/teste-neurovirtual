using System.Data.Entity;
using DAL.RelatoriosAssociados;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<AssociadoEnderecoVW> AssociadoEnderecoVW { get; set; }

		public DbSet<RelatorioAssociadoInadimplenteVW> RelatorioAssociadoInadimplenteVW { get; set; }

		//
		private void mapperModuloRelatoriosAssociados(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new AssociadoEnderecoVWMapper());

			modelBuilder.Configurations.Add(new RelatorioAssociadoInadimplenteVWMapper());
		}
	}
}