using System.Data.Entity;
using DAL.Publicacoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
		public DbSet<TipoNoticia> TipoNoticia { get; set; }
		public DbSet<CategoriaNoticia> CategoriaNoticia { get; set; }
        public DbSet<Noticia> Noticia { get; set; }
		public DbSet<GaleriaFoto> GaleriaFoto { get; set; }
		public DbSet<Video> Video { get; set; }
        public DbSet<Parceiro> Parceiro { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<TipoParceiro> TipoParceiro { get; set; }
        public DbSet<TipoGaleriaFoto> TipoGaleriaFoto { get; set; }
        public DbSet<Jornal> Jornal { get; set; }
		public DbSet<Conteudo> Conteudo { get; set; }
		
		/**
		*
		*/
		private void mapperModuloPublicacoes(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new TipoNoticiaMapper());
			modelBuilder.Configurations.Add(new CategoriaNoticiaMapper());
            modelBuilder.Configurations.Add(new NoticiaMapper());
			modelBuilder.Configurations.Add(new GaleriaFotoMapper());
			modelBuilder.Configurations.Add(new VideoMapper());
            modelBuilder.Configurations.Add(new ParceiroMapper());
            modelBuilder.Configurations.Add(new BannerMapper());
            modelBuilder.Configurations.Add(new TipoParceiroMapper());
            modelBuilder.Configurations.Add(new TipoGaleriaFotoMapper());
            modelBuilder.Configurations.Add(new JornalMapper());
			modelBuilder.Configurations.Add(new ConteudoMapper());
		}
	}
}