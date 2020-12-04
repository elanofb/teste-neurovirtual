using System;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos {

    public interface ITituloDespesaCloneFacadeBL {

        UtilRetorno clonar(TituloDespesa ODespesa, int qtdeReplicacoes);
        
    }
    
}