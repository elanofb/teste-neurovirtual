using System.Data.Entity;
using DAL.RamosAtividade;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<RamoAtividade> RamoAtividade { get; set; }

        public DbSet<SetorAtuacao> SetorAtuacao { get; set; }

		private void mapperModuloRamosAtividade(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new RamoAtividadeMapper());

            modelBuilder.Configurations.Add(new SetorAtuacaoMapper());
		}
	}
}