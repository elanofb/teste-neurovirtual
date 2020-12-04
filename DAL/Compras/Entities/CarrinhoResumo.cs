using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Compras {

	public class CarrinhoResumo {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

		public int? idPessoa { get; set; }

		public string idSessao { get; set;}

		public string cepDestino { get; set;}

        public string logradouroEntrega { get; set;}

        public string numeroEntrega { get; set;}

        public string complementoEntrega { get; set;}

        public string bairroEntrega { get; set;}

        public int? idCidadeEntrega { get; set;}

        public int? idEstadoEntrega { get; set;}

        public string nomeCidadeEntrega { get; set; }

        public string siglaEstadoEntrega { get; set; }

		public int? idTipoFrete { get; set; }

		public decimal valorFrete { get; set;}

        public bool? flagFreteGratis { get; set;}

		public int? prazoEntrega { get; set;}

		public decimal valorDesconto { get; set;}

		public DateTime? dtExclusao { get; set;}

		public DateTime? dtAlteracao { get; set;}

        public IList<CarrinhoItem> listaItens { get; set; }

		public CarrinhoResumo() {

			this.listaItens = new List<CarrinhoItem>();
		}


	}

	//
	internal sealed class CarrinhoResumoMapper : EntityTypeConfiguration<CarrinhoResumo> {

		public CarrinhoResumoMapper() {

			this.ToTable("tb_carrinho_resumo");
			
			this.HasKey(o => o.id);

			this.Ignore(o => o.listaItens);
		}
	}
}
