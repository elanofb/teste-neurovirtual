using System.Data.Entity;
using DAL.BoletosBancarios;


namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<BoletoLayout> BoletoLayout { get; set; }

		private void mapperModuloBoletosBancarios(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new BoletoLayoutMapper());


		}
	}
}