using System;

namespace DAL.RelatoriosPedidos {

	public class SpRelatorioPedidosPorPeriodo {

		public int id { get; set; }

		public string nomePessoa { get; set; }

		public string cpf { get; set; }

		public string rg { get; set; }

		public string email { get; set; }

		public string telPrincipal { get; set; }

		public string telSecundario { get; set; }

        public string descricaoStatusPedido { get; set; }

        public decimal valorProdutos { get; set; }

        public decimal? valorFrete { get; set; }

        public decimal? valorDesconto { get; set; }

        public decimal valorTotal { get; set; }

        public DateTime dtPedido { get; set; }

        public DateTime? dtQuitacao { get; set; }

        public DateTime? dtAtendimento { get; set; }

        public DateTime? dtExpedicao { get; set; }

        public DateTime? dtCancelamento { get; set; }

        public DateTime? dtFinalizado { get; set; }

        public string tipoEnderecoEntrega { get; set; }

        public string cepEnderecoEntrega { get; set; }

        public string logradouroEnderecoEntrega { get; set; }

        public string numeroEnderecoEntrega { get; set; }

        public string complementoEnderecoEntrega { get; set; }

        public string bairroEnderecoEntrega { get; set; }

        public string nomeCidadeEnderecoEntrega { get; set; }

        public string ufEnderecoEntrega { get; set; }

        public string transportadorEntrega { get; set; }

        public string nroRastreamentoEntrega { get; set; }

        public string descricaoTipoFrete { get; set; }

        public string descricaoTipoFreteCliente { get; set; }

        public string produtosPedido { get; set; }

	}
}