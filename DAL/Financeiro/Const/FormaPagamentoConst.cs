using System;

namespace DAL.Financeiro {

	public static class FormaPagamentoConst {

        public static readonly byte BOLETO_BANCARIO = (int)FormaPagamentoEnum.BOLETO_BANCARIO;
        
		public static readonly byte DINHEIRO = (byte)FormaPagamentoEnum.DINHEIRO;
        
		public static readonly byte CHEQUE = (byte)FormaPagamentoEnum.CHEQUE;
        
		public static readonly byte DEPOSITO_BANCARIO = (byte)FormaPagamentoEnum.DEPOSITO_BANCARIO;
        
		public static readonly byte VISA_CREDITO = (byte)FormaPagamentoEnum.VISA_CREDITO;
        
		public static readonly byte VISA_ELECTRON = (byte)FormaPagamentoEnum.VISA_ELECTRON;
        
		public static readonly byte MASTERCARD = (byte)FormaPagamentoEnum.MASTERCARD;
        
		public static readonly byte REDESHOP = (byte)FormaPagamentoEnum.REDESHOP;
        
		public static readonly byte DINERS = (byte)FormaPagamentoEnum.DINERS;
        
		public static readonly byte AMERICAN_EXPRESS = (byte)FormaPagamentoEnum.AMERICAN_EXPRESS;
        
		public static readonly byte HIPERCARD = (byte)FormaPagamentoEnum.HIPERCARD;
        
		public static readonly byte ELO = (byte)FormaPagamentoEnum.ELO;

        public static readonly byte AURA = (byte)FormaPagamentoEnum.AURA;

        public static readonly byte DISCOVER = (byte)FormaPagamentoEnum.DISCOVER;

        public static readonly byte JCB = (byte)FormaPagamentoEnum.JCB;

		public static readonly byte TRANSFERENCIA_BANCARIA = (byte)FormaPagamentoEnum.TRANSFERENCIA_BANCARIA;

		public static readonly byte GUIA = (byte)FormaPagamentoEnum.GUIA;

		public static readonly byte DEBITO_CONTA = (byte)FormaPagamentoEnum.DEBITO_CONTA;

        public static readonly byte DESCONTO_INTEGRAL = (byte)FormaPagamentoEnum.DESCONTO_INTEGRAL;
		
		public static readonly byte BITLINK = (byte)FormaPagamentoEnum.BITLINK;

	}
}