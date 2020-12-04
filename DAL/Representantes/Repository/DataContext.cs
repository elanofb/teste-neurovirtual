using System.Data.Entity;
using DAL.Representantes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

        public DbSet<Representante> Representante { get; set; }

        //
        private void mapperModuloRepresentantes(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new RepresentanteMapper());
		}
	}
}
