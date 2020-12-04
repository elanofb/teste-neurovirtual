using System.Data.Entity;
using DAL.LogsPermissao;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<LogUsuarioSistemaAcesso> LogUsuarioSistemaAcesso { get; set; }

		//
		private void mapperModuloLogsPermissao(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new LogUsuarioSistemaAcessoMapper());
		}
	}
}