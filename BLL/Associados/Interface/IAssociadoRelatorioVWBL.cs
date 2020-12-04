using System.Collections.Specialized;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoRelatorioVWBL {

        AssociadoRelatorioVW carregar(int id);

        IQueryable<AssociadoRelatorioVW> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo);
	}
}
