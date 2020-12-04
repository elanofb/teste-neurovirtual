using System.Json;

namespace BLL.Notificacoes {

    public interface IPessoaDeviceExclusaoBL {

	    JsonMessage excluir(int[] ids);

    }
    
}