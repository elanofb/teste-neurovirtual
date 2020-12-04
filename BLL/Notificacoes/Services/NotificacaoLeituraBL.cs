using System;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using EntityFramework.Extensions;

namespace BLL.Notificacoes.Services {

    public class NotificacaoLeituraBL : DefaultBL, INotificacaoLeituraBL {


        /// <summary>
        /// Registrar a leitura da mensagem
        /// </summary>
        public void registrarLeitura(int idNotificacaoPessoa) {

            db.NotificacaoSistemaEnvio
                            .Where(x => x.idOrganizacao == idOrganizacao && x.id == idNotificacaoPessoa)
                            .Update(x => new NotificacaoSistemaEnvio { dtLeitura = DateTime.Now });

        }
    }
}
