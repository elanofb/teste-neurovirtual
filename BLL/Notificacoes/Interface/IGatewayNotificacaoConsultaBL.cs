using System.Linq;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface IGatewayNotificacaoConsultaBL {

        GatewayNotificacao carregar(int id);

        IQueryable<GatewayNotificacao> listar(string valorBusca, bool? ativo = true);

    }
    
}
