using System.Linq;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

	public interface IAssociadoContribuicaoBL {

		IQueryable<AssociadoContribuicao> query(int? idOrganizacaoParam = null);
		
		AssociadoContribuicao carregar(int id);

		IQueryable<AssociadoContribuicao> listar(int idContribuicao, int idAssociado, bool? flagIsento, bool? flagPago, string valorBusca = "");

	}
}
