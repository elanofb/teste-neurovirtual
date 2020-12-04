using System.Data.Entity.ModelConfiguration;
using DAL.Arquivos;

namespace DAL.Compras {

	public class CarrinhoItemProdutoVW {

        public int id { get; set; }

        public int idOrganizacao { get; set; }

        public int? idPessoa { get; set; }

        public int? idProduto { get; set; }

        public byte qtde { get; set; }

        public bool? flagComprado { get; set; }

        public string descricaoItem { get; set; }

        public decimal valorUnitario { get; set; }

        public decimal? valorDescontoUnitario { get; set; }

        public string idSessao { get; set; }

        public decimal? valorProduto { get; set; }

        public decimal? valorDescontoAssociado { get; set; }

        public decimal? percentualDescontoAssociado { get; set; }

        public bool? flagSomenteAssociados { get; set; }

        public bool? flagSomenteAssociadosAdimplentes { get; set; }

        public bool? flagParaTodos { get; set; }

        public bool? flagCalcularFrete { get; set; }

        public bool? flagFreteGratis { get; set; }

        public bool? flagCortesia { get; set; }

		public decimal? peso { get; set;}

		public decimal? altura { get; set;}

		public decimal? largura { get; set;}

		public decimal? comprimento { get; set;}

        public ArquivoUpload Foto { get; set; }
	}

	//
	internal sealed class CarrinhoItemProdutoVWMapper : EntityTypeConfiguration<CarrinhoItemProdutoVW> {

		public CarrinhoItemProdutoVWMapper() {

			this.ToTable("vw_carrinho_item_produto");
			
			this.HasKey(o => o.id);

		    this.Ignore(x => x.Foto);
		}
	}
}
