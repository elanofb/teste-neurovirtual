using System.Data.Entity.ModelConfiguration;
using System;

namespace DAL.Produtos {

	//
	public class ProdutoRedeConfiguracao {

        public int id { get; set; }

		public int idProduto { get; set; }

		public byte nivel { get; set; }

        public decimal percentualComissao { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }


    }

	//
	internal sealed class ProdutoRedeConfiguracaoMapper : EntityTypeConfiguration<ProdutoRedeConfiguracao> {

		public ProdutoRedeConfiguracaoMapper() {
			
			this.ToTable("tb_produto_rede_configuracao");
			
			this.HasKey(o => o.id);
		}
	}
}