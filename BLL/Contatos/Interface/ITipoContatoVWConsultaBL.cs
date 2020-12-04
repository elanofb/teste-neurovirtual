using DAL.Contatos;
using System.Linq;

namespace BLL.Contatos {

	public interface ITipoContatoVWConsultaBL {

		IQueryable<TipoContatoVW> query(int? idOrganizacaoParam = null);

		TipoContatoVW carregar(int id);

		IQueryable<TipoContatoVW> listar(string valorBusca, bool? ativo = true);

	}
}