using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Emails;

namespace DAL.Email.Extensions {
    
    public static class MensagemEmailExtensions {

        /// <summary>
        /// Retornar lista de e-mails que devem receber cópia da mensagem
        /// </summary>
        public static List<string> listaCopias(this MensagemEmail OMensagemEmail) {
            
            var listaEmails = new List<string>();

            if (OMensagemEmail == null || OMensagemEmail.emailsCopia.isEmpty()) {
                
                return listaEmails;
                
            }

            listaEmails = OMensagemEmail.emailsCopia.Split(';')
                                                    .Where(UtilValidation.isEmail)
                                                    .ToList();
            
            return listaEmails;

        }
        
        
        /// <summary>
        /// Retornar lista de e-mails que devem receber cópia da mensagem
        /// </summary>
        public static bool temConfiguracao(this MensagemEmail OMensagemEmail) {
            
            if (OMensagemEmail == null) {
                
                return false;
                
            }


            if (OMensagemEmail.titulo.isEmpty() || OMensagemEmail.corpoEmail.isEmpty()) {
                
                return false;
            }
            
            return true;

        }
    }
}
