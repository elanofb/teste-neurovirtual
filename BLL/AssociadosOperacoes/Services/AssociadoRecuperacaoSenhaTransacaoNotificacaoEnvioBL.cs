using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Emails;
using BLL.Notificacoes;
using DAL.Notificacoes;
using BLL.Services;
using EntityFramework.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoRecuperacaoSenhaTransacaoNotificacaoEnvioBL : DefaultBL, IAssociadoRecuperacaoSenhaTransacaoNotificacaoEnvioBL {
        
        //Construtor
        public AssociadoRecuperacaoSenhaTransacaoNotificacaoEnvioBL() {

        }

        // 1 - carregar e-mails que devem ser copiados
        // 2 - Chamar servico de disparo de e-mail
        // 3 - Registrar o envio
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
                db.NotificacaoSistemaEnvio.Where(x => listaIdsEnviados.Contains(x.id))
                    .Update(x => new NotificacaoSistemaEnvio() { dtEnvioEmail = DateTime.Now, flagEnvioEmail = true });
            }

            if (listaIdsExcluidos.Any()) {
                db.NotificacaoSistemaEnvio.Where(x => listaIdsExcluidos.Contains(x.id))
                    .Update(x => new NotificacaoSistemaEnvio() { flagExcluido = true, motivoExclusao = "Os e-mails configurados não são válidos." });
            }

        }

        // 1 - Chamada do servico para disparo do e-mail
        private UtilRetorno enviarEmail(NotificacaoSistemaEnvio ONotificacaoSistemaEnvio, List<string> listaEmails) {

            IEnvioRecuperacaoSenhaTransacao EnvioEmail = EnvioRecuperacaoSenhaTransacao.factory(ONotificacaoSistemaEnvio.idOrganizacao, listaEmails, null);

            return EnvioEmail.enviar(ONotificacaoSistemaEnvio);

        }

    }

}
