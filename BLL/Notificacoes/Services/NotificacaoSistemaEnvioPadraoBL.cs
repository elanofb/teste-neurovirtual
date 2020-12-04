using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using EntityFramework.Extensions;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaEnvioPadraoBL : DefaultBL, INotificacaoSistemaEnvioPadraoBL {
	    
        // 1 - Carregar uma cobranca registrada
		// 2 - Carregar os dados da anuidade
		// 3 - carregar e-mails que devem ser copiados
		// 4 - Chamar servico de disparo de e-mail
		// 5 - Registrar o envio
		public void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios) {
            
            var listaIdsEnviados = new List<int>();
            var listaIdsExcluidos = new List<int>();

		    foreach (var ONotificacaoEnvio in listaEnvios) {

			    if (!UtilValidation.isEmail(ONotificacaoEnvio.email)) {
				    
                    listaIdsExcluidos.Add(ONotificacaoEnvio.id);
				    
				    continue;
			    }

			    var ORetorno = enviarEmail(ONotificacaoEnvio, ONotificacaoEnvio.ToEmailList());

			    //Regisrar o envio do e-mail
			    if (!ORetorno.flagError) { 
                    listaIdsEnviados.Add(ONotificacaoEnvio.id);
			    }
			    
			    if (ORetorno.flagError) { 
				    listaIdsExcluidos.Add(ONotificacaoEnvio.id);
			    }

            }

            if (listaIdsEnviados.Any()) {
	            
                db.NotificacaoSistemaEnvio
										.Where(x => listaIdsEnviados.Contains(x.id))
										.Update(x => new NotificacaoSistemaEnvio { dtEnvioEmail = DateTime.Now, flagEnvioEmail = true });
            }

            if (listaIdsExcluidos.Any()) {
                
	            db.NotificacaoSistemaEnvio.Where(x => listaIdsExcluidos.Contains(x.id))
										.Update(x => new NotificacaoSistemaEnvio {
											flagExcluido = true, 
											motivoExclusao = "Os e-mails configurados não são válidos."
										});
            }

		}

		// 1 - Chamada do servico para disparo do e-mail
		public UtilRetorno enviarEmail(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio, List<string> listaEmails) {

			IEnvioEmailNotificacao EnvioEmail = EnvioEmailNotificacao.factory(oNotificacaoSistemaEnvio.idOrganizacao, listaEmails, new List<string>());

			return EnvioEmail.enviar(oNotificacaoSistemaEnvio);
		}

    }

}
