namespace DAL.RelatoriosPedidos {

	public class ItemResumoProdutoPedidoDTO {

        public int id { get; set; }
        public string nome { get; set; }    
        public string tipoProduto { get; set; }		
		public int qtdePedidos { get; set; }
		public int qtdeVendidos { get; set; }
		public decimal valorTotalVendido { get; set; }
		public decimal valorMedio { get; set; }
		public decimal valorMedioPedido { get; set; }
		public int qtdeMinPedido { get; set; }
		public int qtdeMaxPedido { get; set; }
		public decimal percentualPresenca { get; set; }
		public decimal percentualValor { get; set; }
		public string siglaUnidadeCliente { get; set; }    
	}
}