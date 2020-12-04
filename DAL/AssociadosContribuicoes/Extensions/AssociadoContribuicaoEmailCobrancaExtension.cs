using System;
using System.Collections.Generic;

namespace DAL.AssociadosContribuicoes {

	public static class AssociadoContribuicaoEmailCobrancaExtension {

        // Cpaturar a string e joga-la dentro de uma lista
        public static List<string> ToEmailList(this AssociadoContribuicaoEmailCobranca Item) {

            List<string> lista = new List<string>();

			if (Item == null) { 
				return lista;
			}

			if (UtilValidation.isEmail(Item.emailPrincipal)) { 
				lista.Add(Item.emailPrincipal);
			}

			if (UtilValidation.isEmail(Item.emailSecundario)) { 
				lista.Add(Item.emailSecundario);
			}

            return lista;
        }

	}

}
