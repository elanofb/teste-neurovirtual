using System.Data.Entity;
using DAL.Diretorias;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

        public DbSet<Diretoria> Diretoria { get; set; }

        public DbSet<DiretoriaVW> DiretoriaVW { get; set; }

        public DbSet<DiretoriaMembro> DiretoriaMembro { get; set; }

        //
        private void mapperModuloDiretorias(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new DiretoriaVWMapper());
            modelBuilder.Configurations.Add(new DiretoriaMapper());
            modelBuilder.Configurations.Add(new DiretoriaMembroMapper());
        }
	}
}