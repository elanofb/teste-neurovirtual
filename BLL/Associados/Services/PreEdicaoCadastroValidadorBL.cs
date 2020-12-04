using System;
using System.Collections.Generic;
using BLL.Services;

namespace BLL.Associados {

	public class PreEdicaoCadastroValidadorBL : DefaultBL, IPreEdicaoCadastroValidadorBL {

        // Atributos        
        
        // Propriedades        
        
        //
        public PreEdicaoCadastroValidadorBL() {
            
        }
        
        //
        public UtilRetorno validarParametrosAcesso(string idOrganizacaoBase64, string idOrganizacaoSha512, string idAssociadoBase64, string idAssociadoSha512) {
            
            if (idOrganizacaoBase64.isEmpty() || idAssociadoBase64.isEmpty()){
                return new UtilRetorno{ flagError = true, listaErros = new List<string>{ "Parâmetros inválidos!" } };
            }
            
            string idOrganizacaoParam = UtilCrypt.toBase64Decode(idOrganizacaoBase64);
            string idAssociadoParam = UtilCrypt.toBase64Decode(idAssociadoBase64);
            
            if (idOrganizacaoSha512 != UtilCrypt.SHA512(idOrganizacaoParam)){
                return new UtilRetorno{ flagError = true, listaErros = new List<string>{ "Parâmetros inválidos!" } };
            }
            
            if (idAssociadoSha512 != UtilCrypt.SHA512(idAssociadoParam)){
                return new UtilRetorno{ flagError = true, listaErros = new List<string>{ "Parâmetros inválidos!" } };
            }
            
            return new UtilRetorno{ flagError = false };
            
        }
        
    }
    
}