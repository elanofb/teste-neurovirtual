using System.Data.Entity;
using DAL.Localizacao;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoRegiao> TipoRegiao { get; set; }

		public DbSet<CepBrasil> CepBrasil { get; set; }

		public DbSet<Cidade> Cidade { get; set; }

		public DbSet<Estado> Estado { get; set; }

		public DbSet<Pais> Pais { get; set; }

		private void mapperModuloLocalizacao(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new TipoRegiaoMapper());

			modelBuilder.Configurations.Add(new CepBrasilMapper());

			modelBuilder.Configurations.Add(new CidadeMapper());

            modelBuilder.Configurations.Add(new EstadoMapper());

            modelBuilder.Configurations.Add(new PaisMapper());
		}
	}
}