using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {
	public interface ICategoriaTipoAssociadoExclusaoBL {
		//*Rotinas de Exclusão*//
		UtilRetorno excluir(int id);
	}
}