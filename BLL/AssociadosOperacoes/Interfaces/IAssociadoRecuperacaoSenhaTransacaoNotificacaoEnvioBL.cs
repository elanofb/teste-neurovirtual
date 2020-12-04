using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoRecuperacaoSenhaTransacaoNotificacaoEnvioBL {

        void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios);

    }

}
