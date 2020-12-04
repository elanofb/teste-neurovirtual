using System;
using System.Json;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface ITipoTituloBL {

		TipoTitulo carregar(int id);

		IQueryable<TipoTitulo> listar(int idInstituicao, string valorBusca, string ativo);

		bool salvar(TipoTitulo OTipoTitulo);

		bool existe(string descricao, int idDesconsiderado);

        JsonMessageStatus alterarStatus(int id);

		UtilRetorno excluir(int id);

	}
}