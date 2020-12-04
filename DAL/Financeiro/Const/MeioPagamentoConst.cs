namespace DAL.Financeiro {

	public static class MeioPagamentoConst {

        public static readonly byte BOLETO_BANCARIO = (byte)MeioPagamentoEnum.BOLETO_BANCARIO;
        
		public static readonly byte DEPOSITO_BANCARIO = (byte)MeioPagamentoEnum.DEPOSITO_BANCARIO;
        
		public static readonly byte CHEQUE = (byte)MeioPagamentoEnum.CHEQUE;
        
		public static readonly byte DINHEIRO = (byte)MeioPagamentoEnum.DINHEIRO;
        
		public static readonly byte CARTAO_DEBITO = (byte)MeioPagamentoEnum.CARTAO_DEBITO;
		
		public static readonly byte CARTAO_CREDITO = (byte)MeioPagamentoEnum.CARTAO_CREDITO;
		
		public static readonly byte TRANSFERENCIA_ELETRONICA = (byte)MeioPagamentoEnum.TRANSFERENCIA_ELETRONICA;

		public static readonly byte PAGSEGURO = (byte)MeioPagamentoEnum.PAGSEGURO;

        public static readonly byte GUIA = (byte)MeioPagamentoEnum.GUIA;

		public static readonly byte DEBITO_CONTA = (byte)MeioPagamentoEnum.DEBITO_CONTA;

        public static readonly byte DESCONTO_INTEGRAL = (byte)MeioPagamentoEnum.DESCONTO_INTEGRAL;
		
		public static readonly byte BITLINK = (byte)MeioPagamentoEnum.BITLINK;

	}
}