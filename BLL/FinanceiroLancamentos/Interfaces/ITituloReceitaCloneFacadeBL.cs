using System;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos {

    public interface ITituloReceitaCloneFacadeBL {

        UtilRetorno clonar(TituloReceita OReceita, int qtdeReplicacoes);
        
    }
    
}