using System;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class NotificadorTask : INotificadorTask {
        
        //Services
        private INotificador Notificador;
        private INotificacaoSistemaConsultaBL NotificacaoConsultaBL;

        /// <summary>
        /// Construtor
        /// </summary>
        public NotificadorTask(INotificador _Notificador, INotificacaoSistemaConsultaBL _NotificacaoSistemaConsultaBL) {
            
            Notificador = _Notificador;

            NotificacaoConsultaBL = _NotificacaoSistemaConsultaBL;
            
        }

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno executar(int idOrganizacaoParam) {

            var ONotificacao = carregarNotificacao(idOrganizacaoParam);

            if (ONotificacao == null) {
                
                return UtilRetorno.newInstance(true, "Não foram encontradas notificações para envio no momento.");
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Carregar a notificacao há mais tempo na fila para enviar
        /// </summary>
        private NotificacaoSistema carregarNotificacao(int idOrganizacaoParam) {

            var dtFiltro = DateTime.Now;

            var ONotificacao = this.NotificacaoConsultaBL.query(idOrganizacaoParam)
                                   .Where(x =>
                                              (x.dtProgramacaoEnvio == null || x.dtProgramacaoEnvio <= dtFiltro) &&
                                              (x.flagEmail == true && x.dtFinalizacaoEnvioEmail == null) ||
                                              (x.flagMobile == true && x.dtFinalizacaoEnvioPush == null)
                                         )
                                   .Select(x => new {
                                                        x.id,
                                                        x.idOrganizacao,
                                                        x.dtProgramacaoEnvio,
                                                        x.titulo,
                                                        x.notificacao,
                                                        x.tituloPush,
                                                        x.notificacaoPush
                                                    })
                                   .OrderBy(x => x.id)
                                   .FirstOrDefault()
                                   .ToJsonObject<NotificacaoSistema>();

            return ONotificacao;
        }
    }

}
