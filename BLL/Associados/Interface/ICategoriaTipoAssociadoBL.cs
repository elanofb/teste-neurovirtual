using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface ICategoriaTipoAssociadoBL {

		CategoriaTipoAssociado carregar(int id, int? idOrganizacaoInf = null);
		IQueryable<CategoriaTipoAssociado> listar(string valorBusca, string ativo);
		bool salvar(CategoriaTipoAssociado OCategoriaTipoAssociado);
		bool existe(string descricao, int idDesconsiderado, int? idOrganizacaoInf = null);

		UtilRetorno excluir(int id);

	}
}