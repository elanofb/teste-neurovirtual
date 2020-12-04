namespace DAL.Financeiro {

	public static class StatusPagamentoConst {

        public static readonly byte ABERTO = (byte)StatusPagamentoEnum.ABERTO;
        
		public static readonly byte AGUARDANDO_PAGAMENTO = (byte)StatusPagamentoEnum.AGUARDANDO_PAGAMENTO;
        
		public static readonly byte PAGO = (byte)StatusPagamentoEnum.PAGO;
        
		public static readonly byte CANCELADO = (byte)StatusPagamentoEnum.CANCELADO;
        
		public static readonly byte ESTORNADO = (byte)StatusPagamentoEnum.ESTORNADO;
        
		public static readonly byte EM_PROCESSAMENTO = (byte)StatusPagamentoEnum.EM_PROCESSAMENTO;

        public static readonly byte RECUSADO = (byte)StatusPagamentoEnum.RECUSADO;

	}
}