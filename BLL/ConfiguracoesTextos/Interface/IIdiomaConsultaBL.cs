using System.Linq;
using DAL.ConfiguracoesTextos;

namespace BLL.ConfiguracoesTextos {

	public interface IIdiomaConsultaBL {

		IQueryable<Idioma> query(int? idOrganizacaoParam = null);

		Idioma carregar(int id);

		IQueryable<Idioma> listar(string valorBusca, bool? ativo = true);

	}
	
}