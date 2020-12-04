using System.Json;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IAtendimentoCadastroBL {

        bool salvar(Atendimento OAtendimento);
	    
        JsonMessageStatus alterarStatus(int id);

    }

}