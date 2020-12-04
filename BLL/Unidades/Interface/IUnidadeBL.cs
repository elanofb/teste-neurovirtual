using System.Json;
using System.Linq;
using System.Web;
using DAL.Unidades;
using System.Collections.Generic;

namespace BLL.Unidades {

	public interface IUnidadeBL {

		Unidade carregar(int id, bool flagCache = false);

		IQueryable<Unidade> listar(string valorBusca, string ativo, bool flagTodasUnidades = true);

		bool salvar(Unidade OUnidade, HttpPostedFileBase Logotipo);

        bool existe(string nroDocumento, int idDesconsiderado);

        JsonMessageStatus alterarStatus(int id);

		JsonMessage delete(int[] id);
	}
}