using System.Linq;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

	public interface IAssociadoContribuicaoCobrancaBL {

		IQueryable<AssociadoContribuicaoEmailCobranca> listar(int idTarefa, bool? flagEnviado);

	}
}
