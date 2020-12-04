using System;
using System.Linq;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos{
    public interface ITituloDespesaPagamentoResumoVWBL {
        IQueryable<TituloDespesaPagamentoResumoVW> listar(int idTituloDespesa);

        IQueryable<TituloDespesaPagamentoResumoVW> listarPagamentoDespesas(string descricao, int idCentroCusto, int idMacroConta, int idContaBancaria, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim);

        IQueryable<TituloDespesaPagamentoResumoVW> listarPagamentoDespesasExcluidas(string descricao, int idCentroCusto, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim, int? idMacroConta, int? idContaBancaria);
    }
}