using System.Data.Entity;

using DAL.Configuracoes;

namespace DAL.Repository.Base {

    public partial class DataContext : DbContext {
        public DbSet<ConfiguracaoEmail> ConfiguracaoEmail { get; set; }

        public DbSet<ConfiguracaoNotificacao> ConfiguracaoNotificacao { get; set; }

        public DbSet<ConfiguracaoContribuicao> ConfiguracaoContribuicao { get; set; }

        public DbSet<ConfiguracaoFinanceiro> ConfiguracaoFinanceiro { get; set; }

        public DbSet<ConfiguracaoPortal> ConfiguracaoPortal { get; set; }

        public DbSet<ConfiguracaoSistema> ConfiguracaoSistema { get; set; }

        public DbSet<ConfiguracaoRotinaAutomatica> ConfiguracaoRotinaAutomatica { get; set; }

        public DbSet<ConfiguracaoTipoCampo> ConfiguracaoTipoCampo { get; set; }

        public DbSet<ConfiguracaoComissao> ConfiguracaoComissao { get; set; }

        public DbSet<ConfiguracaoPerfilComissionavel> ConfiguracaoPerfilComissionavel { get; set; }

        public DbSet<ConfiguracaoOperacaoCompra> ConfiguracaoOperacaoCompra { get; set; }

        public DbSet<ConfiguracaoSaque> ConfiguracaoSaque { get; set; }

        public DbSet<ConfiguracaoPromocao> ConfiguracaoPromocao { get; set; }

        private void mapperModuloConfiguracao(DbModelBuilder modelBuilder) {
            
            modelBuilder.Configurations.Add(new ConfiguracaoEmailMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoNotificacaoMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoContribuicaoMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoFinanceiroMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoPortalMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoSistemaMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoRotinaAutomaticaMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoTipoCampoMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoComissaoMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoPerfilComissionavelMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoOperacaoCompraMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoSaqueMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoPromocaoMapper());
        }
    }

}