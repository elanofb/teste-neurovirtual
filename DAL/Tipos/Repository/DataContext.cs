using System.Data.Entity;
using DAL.Tipos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoDependente> TipoDependente { get; set; }

		//
		private void mapperModuloTipos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new TipoDependenteMapper());
		}
	}
}