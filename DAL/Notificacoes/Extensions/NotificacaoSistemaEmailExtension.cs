using DAL.Notificacoes;
using System;
using System.Collections.Generic;

namespace DAL.Notificacoes {

	public static class NotificacaoSistemaEmailExtension {

        // Cpaturar a string e joga-la dentro de uma lista
        public static List<string> ToEmailList(this NotificacaoSistemaEnvio Item) {

            List<string> lista = new List<string>();

			if (Item == null) { 
				return lista;
			}

			if (!Item.email.isEmpty() && UtilValidation.isEmail(Item.email)) { 
				lista.Add(Item.email);
			}

            return lista;
        }

	}

}
