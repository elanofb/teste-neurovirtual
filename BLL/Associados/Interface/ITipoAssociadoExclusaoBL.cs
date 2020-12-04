using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {
	public interface ITipoAssociadoExclusaoBL {
        UtilRetorno excluir(int id);
	}
}