using System.Data.Entity;
using DAL.ConfiguracoesTextos;

namespace DAL.Repository.Base {
    public partial class DataContext : DbContext {

        public DbSet<ConfiguracaoTextoCategoria> ConfiguracaoTextoCategoria { get; set; }

        public DbSet<ConfiguracaoTexto> ConfiguracaoTexto { get; set; }
        public DbSet<ConfiguracaoLabel> ConfiguracaoLabel { get; set; }
        public DbSet<Idioma> Idioma { get; set; }

        private void mapperModuloConfiguracoesTextos(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new ConfiguracaoTextoCategoriaMapper());
            modelBuilder.Configurations.Add(new ConfiguracaoTextoMapper());
            modelBuilder.Configurations.Add(new ConfiguracaoLabelMapper());
            modelBuilder.Configurations.Add(new IdiomaMapper());
        }
    }
}
