using System;
using DAL.Financeiro;
using System.Linq;

namespace BLL.Financeiro {

	public interface ITituloReceitaPagamentoCancelamentoBL{

	    UtilRetorno cancelarPagamento(int id);
	}
}
