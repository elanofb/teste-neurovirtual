using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Emails;
using DAL.Notificacoes;
using BLL.Services;
using DAL.Associados;
using EntityFramework.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoNovaSenhaNotificacaoEnvioBL : DefaultBL, IAssociadoNovaSenhaNotificacaoEnvioBL {
        
        //Construtor
        public AssociadoNovaSenhaNotificacaoEnvioBL() {

        }

        // 1 - carregar e-mails que devem ser copiados
        // 2 - Chamar servico de disparo de e-mail
        // 3 - Registrar o envio
        public void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios) {

            var listaIdsEnviados = new List<int>();

            var listaIdsExcluidos = new List<int>();

            var listaIdsRemovidos = new List<int>();

            var idsAssociados = listaEnvios.Select(x => x.idReferencia.toInt()).ToList();

            var listaAssociados = db.Associado.condicoesSeguranca().Where(x => idsAssociados.Contains(x.id))
                                    .Select(x => new { x.id, x.idOrganizacao, Pessoa = new { x.Pessoa.nome, x.Pessoa.login } })
                                    .ToListJsonObject<Associado>();

            var ONotificacao = listaEnvios.FirstOrDefault().NotificacaoSistema;

            foreach (var ONotificacaoEnvio in listaEnvios) {

                var OAssociado = listaAssociados.FirstOrDefault(x => x.id == ONotificacaoEnvio.idReferencia);

                if(OAssociado == null) {

                    listaIdsRemovidos.Add(ONotificacaoEnvio.id);

                    continue;

                }

                if (!UtilValidation.isEmail(ONotificacaoEnvio.email)) {

                    listaIdsExcluidos.Add(ONotificacaoEnvio.id);

                    continue;
                }

                var ORetorno = enviarEmail(OAssociado, ONotificacao, ONotificacaoEnvio.ToEmailList());

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

            if(listaIdsRemovidos.Any()) {

                db.NotificacaoSistemaEnvio
                  .Where(x => listaIdsRemovidos.Contains(x.id))
                  .Update(x => new NotificacaoSistemaEnvio() { flagExcluido = true, motivoExclusao = "O associado informado não existe ou não foi localizado." });
            }

        }

        // 1 - Chamada do servico para disparo do e-mail
        private UtilRetorno enviarEmail(Associado OAssociado, NotificacaoSistema ONotificacao, List<string> listaEmails) {

            IEnvioNovaSenha EnvioEmail = EnvioNovaSenha.factory(OAssociado.idOrganizacao, listaEmails, null);

            return EnvioEmail.enviar(OAssociado, ONotificacao);

        }

    }

}
