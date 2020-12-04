using System.Data.Entity;
using DAL.Notificacoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<NotificacaoSistema> NotificacaoSistema { get; set; }
        
        public DbSet<NotificacaoSistemaEnvio> NotificacaoSistemaEnvio { get; set; }

        public DbSet<TipoNotificacao> TipoNotificacao { get; set; }
		
		public DbSet<GatewayNotificacao> GatewayNotificacao { get; set; }
		
		public DbSet<PessoaDevice> PessoaDevice { get; set; }
		
		public DbSet<ConfiguracaoEmailUsuario> ConfiguracaoEmailUsuario { get; set; }
		
		public DbSet<TemplateMensagem> TemplateMensagem { get; set; }
		
		public DbSet<NotificacaoPostback> NotificacaoPostback { get; set; }

        //
        private void mapperModuloNotificacoes(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new NotificacaoSistemaMapper());
            
            modelBuilder.Configurations.Add(new NotificacaoSistemaEnvioMapper());
	        
            modelBuilder.Configurations.Add(new TipoNotificacaoMapper());
	        
	        modelBuilder.Configurations.Add(new GatewayNotificacaoMapper());
	        
	        modelBuilder.Configurations.Add(new PessoaDeviceMapper());
	        
	        modelBuilder.Configurations.Add(new ConfiguracaoEmailUsuarioMapper());
	        
	        modelBuilder.Configurations.Add(new TemplateMensagemMapper());
	        
	        modelBuilder.Configurations.Add(new NotificacaoPostbackMapper());

		}
	}
}