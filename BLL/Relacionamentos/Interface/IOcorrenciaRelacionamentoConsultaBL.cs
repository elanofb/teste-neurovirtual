using System.Linq;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {

	public interface IOcorrenciaRelacionamentoConsultaBL {

		IQueryable<OcorrenciaRelacionamento> query(int? idOrganizacaoParam = null);

		OcorrenciaRelacionamento carregar(int id);

		IQueryable<OcorrenciaRelacionamento> listar(string valorBusca, bool? ativo = true);

	}
	
}