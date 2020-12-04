using System.Linq;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

	public interface IConfiguracaoEmailUsuarioConsultaBL {

		ConfiguracaoEmailUsuario carregar(int idOrganizacaoParam = 0, int idUsuarioParam = 0, bool flagCache = true);

        IQueryable<ConfiguracaoEmailUsuario> listar(int idOrganizacaoParam, int idUsuarioParam);
		
	}
}