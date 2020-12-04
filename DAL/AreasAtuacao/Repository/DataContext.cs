using System.Data.Entity;
using DAL.AreasAtuacao;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<AreaAtuacao> AreaAtuacao { get; set; }
		public DbSet<AreaAtuacaoTipoAssociado> AreaAtuacaoTipoAssociado { get; set; }

		/**
		*
		*/

		private void mapperModuloAreasAtuacao(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new AreaAtuacaoMapper());
			modelBuilder.Configurations.Add(new AreaAtuacaoTipoAssociadoMapper());
		}
	}
}