using System.Data.Entity;
using DAL.CuponsDesconto;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<CupomDesconto> CupomDesconto { get; set; }

		//
		private void mapperModuloCupomDesconto(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new CupomDescontoMapper());
		}
	}
}