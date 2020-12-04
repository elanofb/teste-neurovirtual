
using System;
using System.Linq;
using DAL.Entities;
using DAL.Pessoas;

namespace DAL.Associados {

    public static class ConfiguracaoMembroExtensions {
               
        public static string descricaoChveBinaria(byte idChaveBinaria) {
            
            if (idChaveBinaria == ChaveBinariaConst.DIREITA) {
                return "Direita";
            }
            
            if (idChaveBinaria == ChaveBinariaConst.ESQUERDA) {
                return "Esquerda";
            }
            
            if (idChaveBinaria == ChaveBinariaConst.MENOR_LADO) {
                return "Menor Lado";
            }

            return "";
        }
        
        
    }

}
