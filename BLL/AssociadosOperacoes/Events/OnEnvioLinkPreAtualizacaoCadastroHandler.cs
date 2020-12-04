using BLL.Core.Events;
using System;
using System.Collections.Generic;
using BLL.Services;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes {

	public class OnEnvioLinkPreAtualizacaoCadastroHandler : DefaultBL, IHandler<object> {
		
		//Atributos Serviços
		private IAtualizacaoCadastroEmailBL _IAtualizacaoCadastroEmailBL;
		
		
		// Propriedades Serviços
		private IAtualizacaoCadastroEmailBL OAtualizacaoCadastroEmailBL => _IAtualizacaoCadastroEmailBL = _IAtualizacaoCadastroEmailBL ?? new AtualizacaoCadastroEmailBL();
		
        //Propriedades
		private NotificacaoSistema ONotificacao { get; set; }

		private List<NotificacaoSistemaEnvio> listaEnvios { get; set; }
		

		//
		public void execute(object source) {

			try {
				
				var listaParametros = (source as List<object>);

				this.ONotificacao = listaParametros[0] as NotificacaoSistema;
			
				this.listaEnvios = listaParametros[1] as List<NotificacaoSistemaEnvio>;
				
				
			} catch (Exception ex) {
				
				UtilLog.saveError(ex, "Erro ao executar capturar os parametros no manipulador OnEnvioLinkPreAtualizacaoCadastroHandler");
				
			}
			
			try {

				this.dispararEmails();

			} catch (Exception ex) {

				UtilLog.saveError(ex, "Erro ao executar o metodo dispararEmails() no manipulador OnEnvioLinkPreAtualizacaoCadastroHandler");

			}
			
		}
		
		/// <summary>
		/// Enviar e-mails
		/// </summary>
		private void dispararEmails() {
	
			this.OAtualizacaoCadastroEmailBL.enviarEmail(this.ONotificacao, this.listaEnvios);

		}

		
	}
	
}