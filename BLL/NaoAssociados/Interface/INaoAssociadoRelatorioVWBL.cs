using System.Collections.Specialized;
using System.Linq;
using DAL.Associados;

namespace BLL.NaoAssociados {

	public interface INaoAssociadoRelatorioVWBL {

        NaoAssociadoRelatorioVW carregar(int id);

        IQueryable<NaoAssociadoRelatorioVW> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo);
	}
}
