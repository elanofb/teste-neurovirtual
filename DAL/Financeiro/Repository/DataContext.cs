using System.Data.Entity;
using DAL.Financeiro;
using DAL.Financeiro.Entities;

namespace DAL.Repository.Base {

	public partial class DataContext {

        public DbSet<CentroCusto> CentroCusto { get; set; }

        public DbSet<MacroConta> MacroConta { get; set; }

        public DbSet<CentroCustoMacroConta> CentroCustoMacroConta { get; set; }

        public DbSet<MeioPagamento> MeioPagamento { get; set; }

		public DbSet<FormaPagamento> FormaPagamento { get; set; }

		public DbSet<GatewayPagamento> GatewayPagamento { get; set; }

		public DbSet<PeriodoRepeticao> PeriodoRepeticao { get; set; }

        public DbSet<CategoriaTitulo> CategoriaTitulo { get; set; }

        public DbSet<TipoReceita> TipoReceita { get; set; }
		
		public DbSet<TituloReceita> TituloReceita { get; set; }

		public DbSet<TituloReceitaPagamento> TituloReceitaPagamento { get; set; }

		public DbSet<StatusPagamento> StatusPagamento { get; set; }
				
		public DbSet<TituloDespesa> TituloDespesa { get; set; }
		
		public DbSet<TituloDespesaPagamento> TituloDespesaPagamento { get; set; }

		public DbSet<TituloReceitaDescontoAntecipacao> TituloReceitaDescontoAntecipacao { get; set; }

		public DbSet<ConciliacaoFinanceira> ConciliacaoFinanceira { get; set; }

		public DbSet<ConciliacaoFinanceiraDetalhe> ConciliacaoFinanceiraDetalhe { get; set; }
		
		public DbSet<TipoDespesa> TipoDespesa { get; set; }
		
		public DbSet<ModoPagamentoDespesa> ModoPagamentoDespesa { get; set; }

        //VIEWS
		public DbSet<TituloReceitaResumoVW> TituloReceitaResumoVW { get; set; }
		
		public DbSet<TituloReceitaPagamentoVW> TituloReceitaPagamentoVW { get; set; }

        public DbSet<GestaoTituloVW> GestaoTituloVW { get; set; }

        public DbSet<TituloReceitaReciboVW> TituloReceitaReciboVW { get; set; }

        public DbSet<TituloDespesaPagamentoResumoVW> TituloDespesaPagamentoResumoVW { get; set; }

        public DbSet<TituloReceitaPagamentoResumoVW> TituloReceitaPagamentoResumoVW { get; set; }

	    public DbSet<ReceitaDespesaVW> ReceitaDespesaVW { get; set; }
		
		public DbSet<ReceitaDespesaArquivoVW> ReceitaDespesaArquivoVW { get; set; }


        //
        private void mapperModuloFinanceiro(DbModelBuilder modelBuilder) {
			
            modelBuilder.Configurations.Add(new CentroCustoMapper());

            modelBuilder.Configurations.Add(new MacroContaMapper());

            modelBuilder.Configurations.Add(new CentroCustoMacroContaMapper());
            
            modelBuilder.Configurations.Add(new MeioPagamentoMapper());
			
			modelBuilder.Configurations.Add(new FormaPagamentoMapper());

			modelBuilder.Configurations.Add(new GatewayPagamentoMapper());
			
			modelBuilder.Configurations.Add(new PeriodoRepeticaoMapper());

            modelBuilder.Configurations.Add(new CategoriaTituloMapper());

            modelBuilder.Configurations.Add(new TipoReceitaMapper());

            modelBuilder.Configurations.Add(new TituloDespesaMapper());

            modelBuilder.Configurations.Add(new TituloDespesaPagamentoMapper());

            modelBuilder.Configurations.Add(new TituloReceitaMapper());

            modelBuilder.Configurations.Add(new TituloReceitaPagamentoMapper());

            modelBuilder.Configurations.Add(new TituloReceitaDescontoAntecipacaoMapper());

	        modelBuilder.Configurations.Add(new StatusPagamentoMapper());

	        modelBuilder.Configurations.Add(new ConciliacaoFinanceiraMapper());
	        
	        modelBuilder.Configurations.Add(new ConciliacaoFinanceiraDetalheMapper());
	        
	        modelBuilder.Configurations.Add(new TipoDespesaMapper());
	        
	        modelBuilder.Configurations.Add(new ModoPagamentoDespesaMapper());

            //VIEWS
			modelBuilder.Configurations.Add(new TituloReceitaResumoVWMapper());
			modelBuilder.Configurations.Add(new TituloReceitaPagamentoVWMapper());
            modelBuilder.Configurations.Add(new GestaoTituloVWMapper());
            modelBuilder.Configurations.Add(new TituloReceitaReciboVWMapper());
            modelBuilder.Configurations.Add(new TituloDespesaPagamentoResumoVWMapper());
            modelBuilder.Configurations.Add(new TituloReceitaPagamentoResumoVWMapper());
            modelBuilder.Configurations.Add(new ReceitaDespesaVWMapper());
	        modelBuilder.Configurations.Add(new ReceitaDespesaArquivoVWMapper());

        }
	}
}
