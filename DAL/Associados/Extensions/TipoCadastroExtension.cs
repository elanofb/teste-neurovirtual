
using System;
using System.Linq;
using DAL.Entities;
using DAL.Pessoas;

namespace DAL.Associados {

    public static class TipoCadastroExtensions {
               
        public static string descricaoTipoCadastro(int idTipoCadastro) {
            
            if (idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE) {
                return "Comerciante";
            }
            
            if (idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR) {
                return "Consumidor";
            }

            return "";
        }
        
        
    }

}
