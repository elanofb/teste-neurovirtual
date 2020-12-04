using BLL.Core.Events;
using System;
using System.Linq;
using DAL.Entities;
using DAL.Notificacoes;

namespace BLL.Notificacoes.Events {

	public class OnNotificacaoExcluidaHandler : IHandler<object> {

		//Atributos
		//Propridades

	    //Chamador das ações do evento
		public void execute(object source) {

			try {

				var idNotificacao = UtilNumber.toInt32(source);

			    if (idNotificacao == 0) {
			        throw new Exception("A notificação informada não foi encontrada.");
			    }

				this.anularTarefaEnvioEmails(idNotificacao);

			} catch (Exception ex) {
				UtilLog.saveError(ex, "Erro no manipulador de evento: OnNotificacaoExcluidaHandler");
			}
		}
        
		// Anular tarefa de envio de emails ao excluir uma notificação
		private void anularTarefaEnvioEmails(int idNotificacao) { 


		}
	}
}