using System.Data.Entity;
using DAL.DadosBancarios;

namespace DAL.Repository.Base {

	public partial class DataContext {

		public DbSet<DadoBancario> DadoBancario { get; set; }
		
		//
		private void mapperModuloDadosBancarios(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new DadoBancarioMapper());

		}
		
	}
}