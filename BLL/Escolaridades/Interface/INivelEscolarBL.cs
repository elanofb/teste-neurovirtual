using System.Linq;
using DAL.Escolaridades;

namespace BLL.Escolaridades {

	public interface INivelEscolarBL {

		IQueryable<NivelEscolar> listar(string valorBusca, string ativo);

	}
}