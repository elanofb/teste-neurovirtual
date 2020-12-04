using System.Data.Entity;
using DAL.Profissoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Profissao> Profissao { get; set; }

		//
		private void mapperModuloProfissoes(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new ProfissaoMapper());
		}
	}
}