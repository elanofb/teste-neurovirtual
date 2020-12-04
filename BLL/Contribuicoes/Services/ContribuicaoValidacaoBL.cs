using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;


namespace BLL.Contribuicoes {

    public class ContribuicaoValidacaoBL : DefaultBL, IContribuicaoValidacaoBL {
        
        //Atributos

        //Propriedades
        
        //
        public UtilRetorno validar(Contribuicao OContribuicao) {

            var ORetorno = UtilRetorno.newInstance(false);
            
            if (OContribuicao.flagBoletoBancarioPermitido == true) {

                
            }
            
            if (OContribuicao.flagCartaoCreditoPermitido == true) {
                
            }
            
            return ORetorno;

        }

    }
    
}