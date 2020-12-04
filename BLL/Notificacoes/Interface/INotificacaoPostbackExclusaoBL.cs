using System.Json;

namespace BLL.Notificacoes {

    public interface INotificacaoPostbackExclusaoBL {

	    JsonMessage excluir(int[] ids);

    }
    
}