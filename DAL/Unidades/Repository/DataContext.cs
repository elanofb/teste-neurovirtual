using System.Data.Entity;
using DAL.Entities;
using DAL.Unidades;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Unidade> Unidade { get; set; }

        public DbSet<UnidadeRota> UnidadeRota { get; set; }

        public DbSet<UsuarioUnidade> UsuarioUnidade { get; set; }

        public DbSet<UnidadeContaBancaria> UnidadeContaBancaria { get; set; }

        public DbSet<UnidadeResumoVW> UnidadeResumoVW { get; set; }

        public DbSet<UnidadeEmissoraVW> UnidadeEmissoraVW { get; set; }

		private void mapperModuloUnidades(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new UnidadeMapper());

            modelBuilder.Configurations.Add(new UsuarioUnidadeMapper());

            modelBuilder.Configurations.Add(new UnidadeContaBancariaMapper());

            modelBuilder.Configurations.Add(new UnidadeRotaMapper());

            modelBuilder.Configurations.Add(new UnidadeResumoVWMapper());

            modelBuilder.Configurations.Add(new UnidadeEmissoraVWMapper());
        }
	}
}