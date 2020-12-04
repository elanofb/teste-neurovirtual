using System.Data.Entity;
using DAL.Associados;

namespace DAL.Repository.Base {

	public partial class DataContext {

		public DbSet<Associado> Associado { get; set; }

		public DbSet<MembroSaldo> MembroSaldo { get; set; }

		public DbSet<MembroSaldoVW> MembroSaldoVW { get; set; }

		public DbSet<AssociadoAreaAtuacao> AssociadoAreaAtuacao { get; set; }

		public DbSet<AssociadoRepresentante> AssociadoRepresentante { get; set; }

		public DbSet<AssociadoAbrangencia> AssociadoAbrangencia { get; set; }

		public DbSet<AssociadoCargo> AssociadoCargo { get; set; }

		public DbSet<AssociadoTitulo> AssociadoTitulo { get; set; }

        public DbSet<AssociadoInstituicao> AssociadoInstituicao { get; set; }

		public DbSet<TipoAssociado> TipoAssociado { get; set; }

		public DbSet<TipoTitulo> TipoTitulo { get; set; }

		public DbSet<TipoAssociadoRepresentante> TipoAssociadoRepresentante { get; set; }
		
        public DbSet<CategoriaTipoAssociado> CategoriaTipoAssociado { get; set; }

        public DbSet<MotivoDesligamento> MotivoDesligamento { get; set; }

        public DbSet<MotivoDesativacao> MotivoDesativacao { get; set; }

        public DbSet<AssociadoRelatorioVW> AssociadoRelatorioVW { get; set; }

        public DbSet<NaoAssociadoRelatorioVW> NaoAssociadoRelatorioVW { get; set; }
        
        public DbSet<PendenciaCadastralVW> PendenciaCadastralVW { get; set; }

        public DbSet<AssociadoEmailVW> AssociadoEmailVW { get; set; }

        public DbSet<ConfiguracaoTipoAssociado> ConfiguracaoTipoAssociado { get; set; }
		
		public DbSet<AssociadoAreaAtuacaoVW> AssociadoAreaAtuacaoVW { get; set; }
		
		public DbSet<ConfiguracaoMembro> ConfiguracaoMembro { get; set; }

        //
        private void mapperModuloAssociados(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new AssociadoMapper());

			modelBuilder.Configurations.Add(new MembroSaldoMapper());

			modelBuilder.Configurations.Add(new MembroSaldoVWMapper());

			modelBuilder.Configurations.Add(new AssociadoAreaAtuacaoMapper());

            modelBuilder.Configurations.Add(new AssociadoAbrangenciaMapper());

            modelBuilder.Configurations.Add(new AssociadoRepresentanteMapper());

            modelBuilder.Configurations.Add(new AssociadoCargoMapper());

            modelBuilder.Configurations.Add(new AssociadoTituloMapper());

            modelBuilder.Configurations.Add(new AssociadoInstituicaoMapper());

            modelBuilder.Configurations.Add(new AssociadoTipoCadastroMapper());

            modelBuilder.Configurations.Add(new TipoAssociadoMapper());

            modelBuilder.Configurations.Add(new TipoTituloMapper());

            modelBuilder.Configurations.Add(new CategoriaTipoAssociadoMapper());

            modelBuilder.Configurations.Add(new TipoAssociadoRepresentanteMapper());

            modelBuilder.Configurations.Add(new MotivoDesligamentoMapper());

            modelBuilder.Configurations.Add(new MotivoDesativacaoMapper());

            modelBuilder.Configurations.Add(new AssociadoRelatorioVWMapper());

            modelBuilder.Configurations.Add(new NaoAssociadoRelatorioVWMapper());
            
            modelBuilder.Configurations.Add(new PendenciaCadastralVWMapper());

            modelBuilder.Configurations.Add(new AssociadoEmailVWMapper());

            modelBuilder.Configurations.Add(new ConfiguracaoTipoAssociadoMapper());
	        
	        modelBuilder.Configurations.Add(new AssociadoAreaAtuacaoVWMapper());
	        
	        modelBuilder.Configurations.Add(new ConfiguracaoMembroMapper());

        }
    }
}