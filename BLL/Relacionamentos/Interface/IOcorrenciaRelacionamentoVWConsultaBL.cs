using System.Linq;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {

	public interface IOcorrenciaRelacionamentoVWConsultaBL {

		IQueryable<OcorrenciaRelacionamentoVW> query(int? idOrganizacaoParam = null);

		OcorrenciaRelacionamentoVW carregar(int id);

		IQueryable<OcorrenciaRelacionamentoVW> listar(string valorBusca, bool? ativo = true);

	}
}