using BLL.Core.Events;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using BLL.AvisosNotificacoes.Services;
using DAL.Notificacoes;

namespace BLL.Notificacoes.Events {

	public class OnNotificacaoCadastradaHandler : IHandler<object> {

		//Atributos Serviços
		
        private INotificacaoSistemaEnvioOperacaoBL _INotificacaoSistemaEnvioOperacaoBL;

		private IMensageiroAppFacadeBL _IMensageiroAppFacadeBL;
        
		//Propridades Serviços
		
        private INotificacaoSistemaEnvioOperacaoBL ONotificacaoSistemaEnvioOperacaoBL => _INotificacaoSistemaEnvioOperacaoBL = _INotificacaoSistemaEnvioOperacaoBL ?? new NotificacaoSistemaEnvioOperacaoBL();
		
		private IMensageiroAppFacadeBL OMensageiroAppFacadeBL => _IMensageiroAppFacadeBL = _IMensageiroAppFacadeBL ?? new MensageiroAppFacadeBL();

		//Propriedades
		private List<NotificacaoSistemaEnvio> listaNotificacoesEnvio;
		
		protected NotificacaoSistema ONotificacaoSistema;
		
		// Constantes
	    private IPrincipal User => HttpContextFactory.Current.User;

        //Chamador das ações do evento
        public void execute(object source) {

			try {

                var listaParametros = source as List<object>;

				this.listaNotificacoesEnvio = (listaParametros[0] as List<NotificacaoSistemaEnvio>);

                this.ONotificacaoSistema = (listaParametros[1] as NotificacaoSistema);

			    if (listaNotificacoesEnvio == null) {
			        throw new Exception("Nenhum item de notificação foi enviado para geração de tarefa.");
			    }


				if (this.ONotificacaoSistema.flagMobile == true) {
					
					this.enviarMensagemApp();
				}

			} catch (Exception ex) {
				UtilLog.saveError(ex, "Erro no manipulador de evento: OnNotificacaoCadastradaHandler");
			}
		}
        

		private void enviarMensagemApp() {

			var ONotificacao = this.ONotificacaoSistema;

			ONotificacao.listaPessoa = this.listaNotificacoesEnvio;

			this.OMensageiroAppFacadeBL.enviar(ONotificacao);

		}

	}
	
}