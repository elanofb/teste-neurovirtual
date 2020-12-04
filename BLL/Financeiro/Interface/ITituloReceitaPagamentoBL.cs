using System;
using System.Collections.Generic;
using DAL.Financeiro;
using System.Linq;

namespace BLL.Financeiro {

	public interface ITituloReceitaPagamentoBL {

		IQueryable<TituloReceitaPagamento> query(int? idOrganizacaoParam = null);
		
        TituloReceitaPagamento carregar(int id, bool? flagExcluido = false);

		IQueryable<TituloReceitaPagamento> listar(int idTituloReceita, bool? flagExcluido = false);

        TituloReceitaPagamento salvar(TituloReceitaPagamento OTituloReceitaPagamento);

	    void conciliarPagamentos(int[] ids, bool flagConciliado);

	    UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta);

		UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta);

	}
}
