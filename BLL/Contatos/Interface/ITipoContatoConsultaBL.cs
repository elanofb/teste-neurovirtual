using DAL.Contatos;
using System.Linq;

namespace BLL.Contatos {

	public interface ITipoContatoConsultaBL {

		IQueryable<TipoContato> query(int? idOrganizacaoParam = null);

		TipoContato carregar(int id);

		IQueryable<TipoContato> listar(string valorBusca, bool? ativo = true);

	}
	
}