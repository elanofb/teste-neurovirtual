using DAL.Financeiro;
using System;
using System.Linq;

namespace BLL.Financeiro {

	public interface ITituloReceitaPagamentoExclusaoBL {
		
	    UtilRetorno excluirPagamento(int idTituloReceitaPagamento, string motivoExclusao, bool? flagAtualizarParcelas = true, string flagOutros = "");

		UtilRetorno excluirParcelaVinculadas(int idTituloReceitaPagamento, string motivoExclusaoParam);
	}
}
