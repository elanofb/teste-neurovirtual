using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.AssociadosOperacoes {
    
    public interface IAtualizacaoCadastroEmailBL {

        void enviarEmail(NotificacaoSistema ONotificacao, List<NotificacaoSistemaEnvio> listaEnvios);

    }
    
}