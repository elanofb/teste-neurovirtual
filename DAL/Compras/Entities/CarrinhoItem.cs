using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Pessoas;
using DAL.Produtos;

namespace DAL.Compras {

	public class CarrinhoItem {

        public int id { get; set; }

        public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

        public int? idProduto { get; set; }

        public virtual Produto Produto { get; set; }

        public int? idItem { get; set; }

		public byte? idTipoItem { get; set; }

        public byte qtde { get; set; }

        public decimal valorUnitario { get; set; }

        public decimal? valorDescontoUnitario { get; set; }

        public decimal? percentualDescontoUnitario { get; set; }

        public bool? flagSomenteAssociados { get; set; }

        public bool? flagSomenteAssociadosAdimplentes { get; set; }

		public decimal? pesoUnitario { get; set;}

        public bool flagCalcularFrete { get; set; }

        public bool? flagFreteGratis { get; set; }

        public bool? flagCortesia { get; set; }

        public string linkImagem { get; set; }

        public string descricaoItem { get; set; }

        public string idSessao { get; set; }

		public bool flagComprado { get; set; }

        public DateTime? dtInclusao { get; set; }

        public DateTime? dtExclusao { get; set; }
	}

	//
	internal sealed class CarrinhoItemMapper : EntityTypeConfiguration<CarrinhoItem> {

		public CarrinhoItemMapper() {

			this.ToTable("tb_carrinho_item");
			
			this.HasKey(o => o.id);

            this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasOptional(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

            this.HasOptional(x => x.Produto).WithMany().HasForeignKey(x => x.idProduto);
		}
	}
}
