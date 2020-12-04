using System.Json;
using DAL.Publicacoes;

namespace BLL.Publicacoes {

	public interface IConteudoCadastroBL {        
        bool salvar(Conteudo OConteudo);
		JsonMessageStatus alterarStatus(int id);
	}
}
