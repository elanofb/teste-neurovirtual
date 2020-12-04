using System.Data.Entity;
using DAL.Institucionais;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<ResultadoBuscaVW> ResultadoBuscaVW { get; set; }

        public DbSet<Convenio> Convenio { get; set; }

        public DbSet<TipoConvenio> TipoConvenio { get; set; }

        public DbSet<AssociacaoHistoria> AssociacaoHistoria { get; set; }


        //
        private void mapperModuloInstitucionais(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new ResultadoBuscaVWMapper());
            modelBuilder.Configurations.Add(new ConvenioMapper());
            modelBuilder.Configurations.Add(new TipoConvenioMapper());
            modelBuilder.Configurations.Add(new AssociacaoHistoriaMapper());
        }
	}
}