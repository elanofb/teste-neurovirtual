using System.Data.Entity;
using DAL.Frete;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Transportador> Transportador { get; set; }

		public DbSet<TipoFrete> TipoFrete { get; set; }

		//Mapeamento banco de dados
		private void mapperModuloFrete(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new TransportadorMapper());

			modelBuilder.Configurations.Add(new TipoFreteMapper());

		}
	}
}