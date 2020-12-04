using System;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos {

    public static class DespesaPagamentoExtensionsBL {
        
        /// <summary>
        /// Retornar Saldo
        /// </summary>
	    public static decimal saldo(this TituloDespesaPagamento ODespesaPagamento) {

            decimal valorNegativo = ODespesaPagamento.valorOriginal.toDecimal() + ODespesaPagamento.valorJuros.toDecimal() + ODespesaPagamento.valorMulta.toDecimal();
            decimal valorPositivo = ODespesaPagamento.valorDesconto.toDecimal() + ODespesaPagamento.valorPago.toDecimal();
            
            return valorPositivo - valorNegativo;

        }
    }
}