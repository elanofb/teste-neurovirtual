using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;


namespace BLL.Contribuicoes {

    public interface IContribuicaoValidacaoBL {
        
        UtilRetorno validar(Contribuicao OContribuicao);

    }
    
}