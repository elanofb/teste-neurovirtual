using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public class ValidadorPessoaEmail : IValidadorPessoaEmail {

        //Atributos
        private IPessoaEmailConsultaBL _EmailConsultaBL;  
            
        //Servicos
        private IPessoaEmailConsultaBL EmailConsultaBL => _EmailConsultaBL = _EmailConsultaBL ?? new  PessoaEmailConsultaBL();

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno validar(List<PessoaEmail> listaEmails, bool flagPermitirDuplicidade) {

            if (listaEmails == null) {
                return UtilRetorno.newInstance(true, "Nenhum e-mail encontrado para validação.");
            }

            if (!listaEmails.Any()) {
                return UtilRetorno.newInstance(true, "Informe um e-mail.");
            }

            var Retorno = new UtilRetorno();

            Retorno.flagError = false;
            
            foreach (var OPessoaEmail in listaEmails) {

                if (!UtilValidation.isEmail(OPessoaEmail.email)) {

                    Retorno.flagError = true;
                    
                    Retorno.listaErros.Add($"O endereço {OPessoaEmail.email} não é válida");
                }
                
                if (flagPermitirDuplicidade) {
                    continue;
                }
                
                var flagExiste = this.EmailConsultaBL.listar(0).Any(x => x.email == OPessoaEmail.email && x.idPessoa != OPessoaEmail.idPessoa);

                if (flagExiste) {

                    Retorno.flagError = true;
                
                    Retorno.listaErros.Add($"O endereço {OPessoaEmail.email} já existe em outro cadastro.");
                    
                }
                
            }

            if (Retorno.flagError) {
                
                return Retorno;
                
            }
                
            return UtilRetorno.newInstance(false, "", listaEmails);
        }
    }

}
