using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.FinanceiroLancamentos {

    public class TituloDespesaPagamentoResumoVWBL : DefaultBL, ITituloDespesaPagamentoResumoVWBL{

        //Listagem de opções de pagamento realizadas
        public IQueryable<TituloDespesaPagamentoResumoVW> listar(int idTituloDespesa) {

            var query = from Tit in this.db.TituloDespesaPagamentoResumoVW
                        .Where(x => x.dtExclusao == null) select Tit;

            query = query.condicoesSeguranca();

            if (idTituloDespesa > 0) {
                query = query.Where(x => x.idTituloDespesa == idTituloDespesa);
            }

            return query;
        }

        public IQueryable<TituloDespesaPagamentoResumoVW> listarPagamentoDespesas(string descricao, int idCentroCusto, int idMacroConta, int idContaBancaria, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim) {

            var query = from OTituloDespesaPag in db.TituloDespesaPagamentoResumoVW
                             .Where(x => x.dtExclusao == null) select OTituloDespesaPag;

            query = query.condicoesSeguranca();
            
            if (!String.IsNullOrEmpty(descricao)) {

                int buscaNumerica = descricao.onlyNumber().toInt();

                if (buscaNumerica > 0){
                    
                    query = query.Where(x => (x.idTituloPagamento == buscaNumerica ||  x.idTituloDespesa == buscaNumerica));
                    
                }else{
                    
                    query = query.Where(x => 
                        (x.descricao.Contains(descricao) || 
                         x.descParcela.Contains(descricao) || 
                         x.nomePessoa.Contains(descricao) || 
                         x.nroDocumentoPessoa == descricao));
                    
                }                                                                
                
            }

            if (idCentroCusto > 0) {
                query = query.Where(x => x.idCentroCusto == idCentroCusto);
            }

            
            if (idMacroConta > 0) {
                query = query.Where(x => x.idMacroConta == idMacroConta);
            }

            if (idContaBancaria > 0) {
                query = query.Where(x => x.idContaBancaria == idContaBancaria);
            }

            if (!String.IsNullOrEmpty(flagPago)) {
                query = (flagPago == "S") ? query.Where(x => x.dtPagamento != null) : query.Where(x => x.dtPagamento == null);
            }

            dtFim = dtFim?.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (pesquisarPor == "P") {
                query = query.Where(x => (x.dtPagamento >= dtInicio) && (x.dtPagamento <= dtFim));
            } else {
                query = query.Where(x => (x.dtVencimentoDespesa >= dtInicio) && (x.dtVencimentoDespesa <= dtFim));
            }

            return query;
        }

        public IQueryable<TituloDespesaPagamentoResumoVW> listarPagamentoDespesasExcluidas(string descricao, int idCentroCusto, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim, int? idMacroConta, int? idContaBancaria) {

            var query = from OTituloDespesaPag in
                             db.TituloDespesaPagamentoResumoVW
                             .Where(x => x.dtExclusao.HasValue)
                        select OTituloDespesaPag;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(descricao)) {
                query = query.Where(x => x.descricao.Contains(descricao));
            }

            if (idCentroCusto > 0) {
                query = query.Where(x => x.idCentroCusto == idCentroCusto);
            }

            if (idMacroConta > 0) {
                query = query.Where(x => x.idMacroConta == idMacroConta);
            }

            if (idContaBancaria > 0) {
                query = query.Where(x => x.idContaBancaria == idContaBancaria);
            }

            if (!String.IsNullOrEmpty(flagPago)) {
                query = (flagPago == "S") ? query.Where(x => x.dtPagamento != null) : query.Where(x => x.dtPagamento == null);
            }

            dtFim = dtFim?.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (pesquisarPor == "P") {
                query = query.Where(x => (x.dtPagamento >= dtInicio) && (x.dtPagamento <= dtFim));
            } else if (pesquisarPor == "E") {
                query = query.Where(x => x.dtExclusao >= dtInicio && x.dtExclusao <= dtFim);
            } else {
                query = query.Where(x => (x.dtVencimentoDespesa >= dtInicio) && (x.dtVencimentoDespesa <= dtFim));
            }

            return query;
        }

    }
}
