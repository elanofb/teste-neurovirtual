using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoCargoBL{
		AssociadoCargo carregar(int id);
		IQueryable<AssociadoCargo> listar(int idAssociado, string ativo);
		bool salvar(AssociadoCargo OAssociadoCargo);
		bool existe(AssociadoCargo OAssociadoCargo, int idDesconsiderar);
		UtilRetorno excluir(int id);
	}
}