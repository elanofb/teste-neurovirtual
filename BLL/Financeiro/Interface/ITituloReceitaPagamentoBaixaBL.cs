using System;
using DAL.Financeiro;

namespace BLL.Financeiro {
    
    public interface ITituloReceitaPagamentoBaixaBL {
        
        UtilRetorno registrarPagamento(TituloReceitaPagamento OTituloReceitaPagamento);
        
        UtilRetorno registrarPagamento(int idTituloReceitaPagamento, DateTime dtPagamento);
    }
}