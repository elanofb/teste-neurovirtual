using System.Data.Entity;
using DAL.MeiosDivulgacao;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<MeioDivulgacao> MeioDivulgacao { get; set; }

		//
		private void mapperModuloMeiosDivulgacao(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new MeioDivulgacaoMapper() );
		}
	}
}