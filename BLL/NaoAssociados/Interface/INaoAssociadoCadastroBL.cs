using System.Collections.Specialized;
using System.Linq;
using DAL.Associados;

namespace BLL.NaoAssociados {

	public interface INaoAssociadoCadastroBL {

        Associado salvar(Associado OAssociado);

	}
}
