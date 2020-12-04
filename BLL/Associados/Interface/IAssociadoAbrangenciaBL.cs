using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoAbrangenciaBL{
		AssociadoAbrangencia carregar(int id);
		IQueryable<AssociadoAbrangencia> listar(int idAssociado, string ativo);
		bool salvar(AssociadoAbrangencia OAssociadoAbrangencia);
		bool existe(AssociadoAbrangencia OAssociadoAbrangencia, int idDesconsiderar);
		UtilRetorno excluir(int idAbrangencia, int idAssociado);
		UtilRetorno excluirPorId(int id);
	}
}