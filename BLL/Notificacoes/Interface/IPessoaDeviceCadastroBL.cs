using System.Json;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface IPessoaDeviceCadastroBL {

	    bool salvar(PessoaDevice OPessoaDevice);

        JsonMessageStatus alterarStatus(int id);

    }
    
}