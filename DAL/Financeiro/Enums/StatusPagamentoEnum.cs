namespace DAL.Financeiro {

	public enum StatusPagamentoEnum {
		
		ABERTO = 1,
		
		AGUARDANDO_PAGAMENTO = 2,
		
		PAGO = 3,
		
		CANCELADO = 4,

		ESTORNADO = 5,

        EM_PROCESSAMENTO = 6,

        RECUSADO = 7
	}
}