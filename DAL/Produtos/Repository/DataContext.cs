using System.Data.Entity;
using DAL.Produtos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoProduto> TipoProduto { get; set; }

        public DbSet<Produto> Produto { get; set; }

        public DbSet<ProdutoTipoAssociado> ProdutoTipoAssociado { get; set; }

        public DbSet<ProdutoEstoque> ProdutoEstoque { get; set; }

        public DbSet<EstoqueEntrada> EstoqueEntrada { get; set; }

        public DbSet<EstoqueSaida> EstoqueSaida { get; set; }

        public DbSet<TipoReferenciaSaida> TipoReferenciaSaida { get; set; }

        public DbSet<UnidadeMedida> UnidadeMedida { get; set; }

        public DbSet<ProdutoItem> ProdutoItem { get; set; }

        public DbSet<ProdutoComposicao> ProdutoComposicao { get; set; }
		
		public DbSet<ProdutoRedeConfiguracao> ProdutoRedeConfiguracao { get; set; }

        public DbSet<ProdutoSituacao> ProdutoSituacao { get; set; }

        //
        private void mapperModuloProdutos(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new TipoProdutoMapper());

            modelBuilder.Configurations.Add(new ProdutoMapper());

            modelBuilder.Configurations.Add(new ProdutoTipoAssociadoMapper());

            modelBuilder.Configurations.Add(new ProdutoEstoqueMapper());

            modelBuilder.Configurations.Add(new EstoqueEntradaMapper());

            modelBuilder.Configurations.Add(new EstoqueSaidaMapper());

            modelBuilder.Configurations.Add(new TipoReferenciaSaidaMapper());

		    modelBuilder.Configurations.Add(new UnidadeMedidaMapper());

		    modelBuilder.Configurations.Add(new ProdutoItemMapper());

		    modelBuilder.Configurations.Add(new ProdutoComposicaoMapper());
			
			modelBuilder.Configurations.Add(new ProdutoRedeConfiguracaoMapper());

            modelBuilder.Configurations.Add(new ProdutoSituacaoMapper());
        }
	}
}