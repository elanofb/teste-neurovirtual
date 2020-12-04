using System.Data.Entity;
using DAL.Instituicoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Instituicao> Instituicao { get; set; }

		/**
		*
		*/

		private void mapperModuloInstituicoes(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new InstituicaoMapper());
		}
	}
}