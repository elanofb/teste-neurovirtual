using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoTituloBL{
		AssociadoTitulo carregar(int id);
		IQueryable<AssociadoTitulo> listar(int idAssociado, int idTipoTitulo, string ativo);
		bool salvar(AssociadoTitulo OAssociadoTitulo);
		bool existe(AssociadoTitulo OAssociadoTitulo, int idDesconsiderar);
		UtilRetorno excluir(int id);
	}
}