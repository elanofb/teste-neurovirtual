using System.Linq;
using DAL.Financeiro;
using System;
using System.Collections.Generic;

namespace BLL.Financeiro {

    public interface ITituloDespesaPagamentoBL {
        //
        TituloDespesaPagamento carregar(int id, bool? flagExcluido = false);

        //
        IQueryable<TituloDespesaPagamento> listar(int idTituloDespesa, bool? flagExcluido = false);

        //
        UtilRetorno registrarPagamento(TituloDespesaPagamento OTituloDespesaPagamento);

        //
        UtilRetorno cancelarPagamento(int id);

        //
        void conciliarPagamentos(int[] ids, bool flagConciliado);

        //
        UtilRetorno excluir(int id, string motivo, string flagOutros = "");

        UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta);

        UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta);
    }
}
