using System;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IConciliacaoFinanceiraCadastroBL {        
		
        bool salvar(ConciliacaoFinanceira OListaConciliacaoFinanceira);
		
		UtilRetorno excluir(ConciliacaoFinanceira OConciliacaoFinanceira);
	}
}
