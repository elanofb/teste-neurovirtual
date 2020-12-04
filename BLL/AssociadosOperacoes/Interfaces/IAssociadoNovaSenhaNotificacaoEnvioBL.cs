using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoNovaSenhaNotificacaoEnvioBL {

        void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios);

    }

}
