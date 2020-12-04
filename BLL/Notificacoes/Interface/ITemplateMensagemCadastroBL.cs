using System.Json;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface ITemplateMensagemCadastroBL {

	    bool salvar(TemplateMensagem OTemplateMensagem);

        JsonMessageStatus alterarStatus(int id);

    }
    
}