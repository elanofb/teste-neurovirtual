using System.Data.Entity;
using DAL.Bancos;

namespace DAL.Repository.Base {

	public partial class DataContext {

		public DbSet<Banco> Banco { get; set; }
		
		//
		private void mapperModuloBancos(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new BancoMapper());

		}
	}
}