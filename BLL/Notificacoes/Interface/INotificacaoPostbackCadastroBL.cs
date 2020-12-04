using System.Json;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface INotificacaoPostbackCadastroBL {

	    bool salvar(NotificacaoPostback ONotificacaoPostback);

        JsonMessageStatus alterarStatus(int id);

    }
    
}