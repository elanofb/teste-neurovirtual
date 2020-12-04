using System.Data.Entity;
using DAL.Contratos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Contrato> Contrato { get; set; }

		public DbSet<StatusContrato> StatusContrato { get; set; }

		public DbSet<TipoContrato> TipoContrato { get; set; }

		/**
		*
		*/

		private void mapperModuloContrato(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new ContratoMapper());
			modelBuilder.Configurations.Add(new StatusContratoMapper());
			modelBuilder.Configurations.Add(new TipoContratoMapper());
		}
	}
}