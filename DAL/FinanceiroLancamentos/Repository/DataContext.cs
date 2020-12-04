using System.Data.Entity;
using DAL.FinanceiroLancamentos;

namespace DAL.Repository.Base {

	public partial class DataContext {

        public DbSet<Credor> Credor { get; set; }

        public DbSet<Devedor> Devedor { get; set; }

        //VIEWS
        public DbSet<CredorVW> CredorVW { get; set; }
        public DbSet<DevedorVW> DevedorVW { get; set; }

        //
        private void mapperModuloFinanceiroLancamentos(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new CredorMapper());
			modelBuilder.Configurations.Add(new DevedorMapper());

            //VIEWS
            modelBuilder.Configurations.Add(new CredorVWMapper());
            modelBuilder.Configurations.Add(new DevedorVWMapper());
        }
	}
}
