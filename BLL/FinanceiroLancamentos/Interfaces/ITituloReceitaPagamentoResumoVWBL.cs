using System;
using System.Linq;
using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos{
    public interface ITituloReceitaPagamentoResumoVWBL {
        IQueryable<TituloReceitaPagamentoResumoVW> listar(int idTituloDespesa);

        IQueryable<TituloReceitaPagamentoResumoVW> listarPagamentoReceitas(string descricao, int idCentroCusto, int idMacroConta, int idContaBancaria, int idTipoReceita, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim);

        IQueryable<TituloReceitaPagamentoResumoVW> listarPagamentoReceitasExcluidas(string descricao, int idCentroCusto, int idTipoReceita, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim, int? idMacroConta, int? idContaBancaria);
    }
}