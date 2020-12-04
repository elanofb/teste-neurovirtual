using System;
using System.Collections.Generic;

namespace BLL.Associados {

	public interface IAssociadoAdmissaoBL {
        
		UtilRetorno admitirAssociados(List<int> idsAssociados, DateTime? dtAdmissao, string observacoes);

	}
}