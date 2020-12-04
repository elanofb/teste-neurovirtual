using System.Linq;
using DAL.Associados;

namespace BLL.AssociadosDependentes {

	public interface IAssociadoDependenteBL {
        
        IQueryable<Associado> listar(int idAssociadoEstipulante, string valorBusca, string ativo);

	    Associado carregar(int idAssociado);
	}
}
