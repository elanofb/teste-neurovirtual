using System.Linq;
using DAL.Financeiro;
using System;
using System.Collections.Generic;

namespace BLL.Financeiro {

	public interface ITituloDespesaBL {

        TituloDespesa carregar(int id, bool? flagExcluido = false);

	    TituloDespesa carregarPorDespesa(int id);

		IQueryable<TituloDespesa> listar(string valorBusca, bool? flagExcluido = false);

	    UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta);

	    UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta);

        UtilRetorno excluir(int id, string motivoExclusao);

	}
}
