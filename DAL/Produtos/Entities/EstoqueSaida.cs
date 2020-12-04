using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Fornecedores;

namespace DAL.Produtos {


    public enum enumReferenciaSaidaEstoque {
        CLIENTES = 1,
        COLABORADORES = 2,
        OUTROS = 3
    }
	//
	public class EstoqueSaida : DefaultEntity {

		public int idReferencia { get; set; }
		public int idProdutoEstoque { get; set; }
		public virtual ProdutoEstoque ProdutoEstoque { get; set; }
        public int idTipoReferenciaSaida { get; set; }
		public virtual TipoReferenciaSaida TipoReferenciaSaida { get; set; }

	}

	//
	internal sealed class EstoqueSaidaMapper : EntityTypeConfiguration<EstoqueSaida> {

		public EstoqueSaidaMapper() {
			this.ToTable("tb_movimentacao_saida");
			this.HasKey(o => o.id);
			this.HasRequired(t => t.ProdutoEstoque).WithMany().HasForeignKey(t => t.idProdutoEstoque);
			this.HasRequired(t => t.TipoReferenciaSaida).WithMany().HasForeignKey(t => t.idTipoReferenciaSaida);
		}
	}
}