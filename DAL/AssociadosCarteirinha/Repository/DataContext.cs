using DAL.AssociadosCarteirinha;
using System.Data.Entity;

namespace DAL.Repository.Base {

    public partial class DataContext {

        public DbSet<AssociadoCarteirinha> AssociadoCarteirinha { get; set; }

		//
		private void mapperModuloAssociadosCarteirinha(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new AssociadoCarteirinhaMapper());

		}
	}
}