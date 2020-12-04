using System.Data.Entity;
using DAL.RedeAfiliados;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<RedeAfiliado> RedeAfiliado { get; set; }
		
		public DbSet<RedeLinearVW> RedeLinear { get; set; }
		
		public DbSet<RedeBinaria> RedeBinaria { get; set; }

		public DbSet<RedePontuacao> RedePontuacao { get; set; }
		
		public DbSet<RedeBinariaEsquerdaVW> RedeBinariaEsquerdaVW { get; set; }
		
		public DbSet<RedeBinariaDireitaVW> RedeBinariaDireitaVW { get; set; }
		
		private void mapperModuloRedes(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new RedeAfiliadoMapper());
			
			modelBuilder.Configurations.Add(new RedeLinearVWMapper());
			
			modelBuilder.Configurations.Add(new RedeBinariaMapper());
			
			modelBuilder.Configurations.Add(new RedePontuacaoMapper());
			
			modelBuilder.Configurations.Add(new RedeBinariaEsquerdaVWMapper());
			
			modelBuilder.Configurations.Add(new RedeBinariaDireitaVWMapper());
		}
	}
}