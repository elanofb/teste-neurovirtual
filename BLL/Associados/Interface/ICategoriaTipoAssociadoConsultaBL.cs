using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface ICategoriaTipoAssociadoConsultaBL {
		//*Rotinas de consulta*//
		CategoriaTipoAssociado carregar(int id, int? idOrganizacaoInf = null);
		IQueryable<CategoriaTipoAssociado> listar(string valorBusca, string ativo);
	}
}