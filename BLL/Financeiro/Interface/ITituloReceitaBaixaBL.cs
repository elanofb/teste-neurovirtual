using System.Collections.Generic;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public interface ITituloReceitaBaixaBL {

        TituloReceita liquidar(int idReceita, List<TituloReceitaPagamento> listaPagamentos, int idUsuarioBaixa);
        
        TituloReceita liquidar(TituloReceita OTituloReceita, List<TituloReceitaPagamento> listaPagamentos);

        TituloReceita liquidar(TituloReceita OTituloReceita);

        
    }
}