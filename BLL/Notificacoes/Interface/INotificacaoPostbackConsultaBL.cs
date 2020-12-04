using System.Linq;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface INotificacaoPostbackConsultaBL {

        IQueryable<NotificacaoPostback> query();

        NotificacaoPostback carregar(int id);

        IQueryable<NotificacaoPostback> listar(bool? ativo = true);

    }
    
}