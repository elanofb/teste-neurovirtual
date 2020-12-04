using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoRecuperacaoSenhaNotificacaoEnvioBL {

        void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios);

    }

}
