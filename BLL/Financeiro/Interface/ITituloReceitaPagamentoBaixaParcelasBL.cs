using System;
using DAL.Financeiro;

namespace BLL.Financeiro{
    
    public interface ITituloReceitaPagamentoBaixaParcelasBL{
        
        /// <summary>
        /// Registrar o pagamento das parcelas adicionais de um titulo, a partir da parcela principal
        /// </summary>
        UtilRetorno registrarPagamento(TituloReceitaPagamento OTituloReceitaPagamento);
    }
}