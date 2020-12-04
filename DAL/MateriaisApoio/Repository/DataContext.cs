using System.Data.Entity;
using DAL.MateriaisApoio;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<MaterialApoio> MaterialApoio { get; set; }
        public DbSet<MaterialApoioPessoa> MaterialApoioPessoa { get; set; }
        public DbSet<TipoMaterialApoio> TipoMaterialApoio { get; set; }

		private void mapperModuloMateriaisApoio(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new MaterialApoioMapper());
            modelBuilder.Configurations.Add(new MaterialApoioPessoaMapper());
            modelBuilder.Configurations.Add(new TipoMaterialApoioMapper());
		}
	}
}