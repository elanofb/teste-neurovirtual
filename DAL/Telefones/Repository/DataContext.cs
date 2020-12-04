using System.Data.Entity;
using DAL.Telefones;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoTelefone> TipoTelefone { get; set; }

        public DbSet<OperadoraTelefonia> OperadoraTelefonia { get; set; }

        //
        private void mapperModuloTelefones(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new TipoTelefoneMapper());

            modelBuilder.Configurations.Add(new OperadoraTelefoniaMapper());
			
		}
	}
}