using System.Json;

namespace BLL.Notificacoes {

    public interface ITemplateMensagemExclusaoBL {

	    JsonMessage excluir(int[] ids);

    }
    
}