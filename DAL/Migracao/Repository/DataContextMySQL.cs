using System.Data.Entity;
using DAL.Mailings;
using DAL.Migracoes;

namespace DAL.Repository.Base {

	public partial class DataContextMySQL : DbContext {
		
		public DbSet<UsuarioMembro> UsuarioMembro { get; set; }
		public DbSet<UsuarioComerciante> UsuarioComerciante { get; set; }
		public DbSet<UsuarioGeral> UsuarioGeral { get; set; }
		public DbSet<Comerciante> Comerciante { get; set; }
		public DbSet<Rede> Rede { get; set; }
		public DbSet<RedePontos> RedePontos { get; set; }
		public DbSet<Extrato> Extrato { get; set; }
		
		/**
		*
		*/
			
		private void mapperModuloMigracao(DbModelBuilder modelBuilder) {
			
			modelBuilder.Configurations.Add(new UsuarioMembroMapper());
			modelBuilder.Configurations.Add(new UsuarioComercianteMapper());
			modelBuilder.Configurations.Add(new UsuarioGeralMapper());
			modelBuilder.Configurations.Add(new ComercianteMapper());
			modelBuilder.Configurations.Add(new RedeMapper());
			modelBuilder.Configurations.Add(new RedePontosMapper());
			modelBuilder.Configurations.Add(new ExtratoMapper());
			
		}
		
	}
}