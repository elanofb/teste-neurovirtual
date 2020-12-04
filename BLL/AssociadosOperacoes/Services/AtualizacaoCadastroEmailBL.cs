using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Emails;
using BLL.Services;
using DAL.Notificacoes;
using EntityFramework.Extensions;

namespace BLL.AssociadosOperacoes {
    
    public class AtualizacaoCadastroEmailBL : DefaultBL, IAtualizacaoCadastroEmailBL {
        
        //Atributos
        
        //Servicos

        //
        public void enviarEmail(NotificacaoSistema ONotificacao, List<NotificacaoSistemaEnvio> listaEnvios) {

            var listaIdsEnviados = new List<int>();
            
            var listaExcluidos = new List<UtilRetorno>();
            
            foreach (var ONotificacaoEnvio in listaEnvios) {

                var listaEmailsEnvio = ONotificacaoEnvio.ToEmailList();

                var flagEnviado = false;

                var ORetorno = UtilRetorno.newInstance(false);
                
                foreach (var stringEmail in listaEmailsEnvio) {
                    
                    ORetorno = this.enviarEmail(ONotificacao, ONotificacaoEnvio, new List<string> { stringEmail });

                    if (!ORetorno.flagError) {
                        
                        flagEnviado = true;
                        
                        continue;
                    }

                    ORetorno.info = ONotificacaoEnvio.id;

                }
                
                if (flagEnviado) {
                    listaIdsEnviados.Add(ONotificacaoEnvio.id);
                }   
                    
                if (!flagEnviado) {
                    listaExcluidos.Add(ORetorno);
                }

            }

            if (listaIdsEnviados.Any()) {
                
                db.NotificacaoSistemaEnvio.Where(x => listaIdsEnviados.Contains(x.id))
                    .Update(x => new NotificacaoSistemaEnvio() { dtEnvioEmail = DateTime.Now, flagEnvioEmail = true });
                
            }

            if (listaExcluidos.Any()) {
            
                var idsEnviosExcluidos = listaExcluidos.Select(x => x.info.toInt()).Distinct().ToList();

                var listaEnviosExcluidos = db.NotificacaoSistemaEnvio.Where(x => idsEnviosExcluidos.Contains(x.id)).ToList(); 
                
                listaEnviosExcluidos.ForEach(x => {

                    x.flagExcluido = true;

                    x.motivoExclusao = listaExcluidos.FirstOrDefault(c => c.info.toInt() == x.id)?.listaErros?.FirstOrDefault() ?? "Os e-mails configurados não são válidos.";

                });

                db.SaveChanges();
                
            }

            var idsMembrosNotificados = listaEnvios.Where(x => listaIdsEnviados.Contains(x.id) && x.idReferencia > 0)
                                                   .Select(x => x.idReferencia.toInt())
                                                   .ToList();

        }
        
        //
        private UtilRetorno enviarEmail(NotificacaoSistema ONotificacao, NotificacaoSistemaEnvio OEnvio, List<string> listaEmails) {
            
            IEnvioAtualizacaoCadastro EnvioEmail = EnvioAtualizacaoCadastro.factory(OEnvio.idOrganizacao, listaEmails, null);
            
            return EnvioEmail.enviar(OEnvio, ONotificacao);

        }


    }
    
    
}