using DAL.Notificacoes;
using System;
using System.Collections.Generic;

namespace DAL.Notificacoes {

	public static class NotificacaoSistemaExtension {

		//
        public static bool flagNotificacaoAssociado(this NotificacaoSistema Item) {

	        if (Item.flagTodosAssociados == true || Item.flagAssociadosEspecificos == true || 
	            Item.flagAssociadosAdimplentes == true || Item.flagAssociadosInadimplentes == true) {

		        return true;
	        }

	        return false;

        }
		
		//
		public static bool flagNotificacaoUsuario(this NotificacaoSistema Item) {

			if (Item.flagTodosUsuarios == true || Item.flagUsuariosEspecificos == true) {

				return true;
			}

			return false;

		}

	}

}
