using System.Data.Entity;
using DAL.Enderecos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoEndereco> TipoEndereco { get; set; }

		/**
		*
		*/

		private void mapperModuloEnderecos(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new TipoEnderecoMapper());
		}
	}
}