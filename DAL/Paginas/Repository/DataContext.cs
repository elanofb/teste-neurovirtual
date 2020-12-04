using System.Data.Entity;
using DAL.Paginas;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        
        public DbSet<PaginaAssocie> PaginaAssocie { get; set; }

        public DbSet<PaginaEstatuto> PaginaEstatuto { get; set; }


        //
        private void mapperModuloPaginas(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new PaginaAssocieMapper());

			modelBuilder.Configurations.Add(new PaginaEstatutoMapper());

        }
	}
}