namespace DAL.Financeiro {

	public enum MeioPagamentoEnum {
		
		BOLETO_BANCARIO = 1,
		
		DEPOSITO_BANCARIO = 2,
		
		CHEQUE = 3,
		
		DINHEIRO = 4,
		
		CARTAO_DEBITO = 5,
		
		CARTAO_CREDITO = 6, 

        TRANSFERENCIA_ELETRONICA = 7,

        PAGSEGURO = 8,

        DESCONTO_INTEGRAL = 9,
		
		BITLINK = 10,

        GUIA = 100,

        DEBITO_CONTA = 101
	}
}