using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.FinanceiroLancamentos {

    public class TituloReceitaPagamentoResumoVWBL : DefaultBL, ITituloReceitaPagamentoResumoVWBL {


        //Listagem de opcoes de pagamento realizadas
        public IQueryable<TituloReceitaPagamentoResumoVW> listar(int idTituloReceita) {

            var query = from Tit in this.db.TituloReceitaPagamentoResumoVW
                        .Where(x => x.dtExclusao == null)
                        select Tit;

            query = query.condicoesSeguranca();

            if (idTituloReceita > 0) {
                query = query.Where(x => x.idTituloReceita == idTituloReceita);
            }

            return query;
        }

        public IQueryable<TituloReceitaPagamentoResumoVW> listarPagamentoReceitas(string descricao, int idCentroCusto, int idMacroConta, int idContaBancaria, int idTipoReceita, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim) {

            var query = from TR in this.db.TituloReceitaPagamentoResumoVW
                                            .Where(x => x.dtExclusao == null)
                        select TR;

            query = query.condicoesSeguranca();
            
            if (!String.IsNullOrEmpty(descricao)) {

                int buscaNumerica = descricao.onlyNumber().toInt();

                if (buscaNumerica > 0){
                    
                    query = query.Where(x => 
                        (x.idTituloPagamento == buscaNumerica ||  x.idTituloReceita == buscaNumerica));
                    
                }else{
                    
                    query = query.Where(x => 
                        (x.descricao.Contains(descricao) || 
                         x.descricaoParcela.Contains(descricao) || 
                         x.nomePessoa.Contains(descricao) || 
                         x.nroDocumentoPessoa == descricao));
                    
                }                                                                
                
            }
            
            if (idCentroCusto > 0) {
                query = query.Where(x => x.idCentroCusto == idCentroCusto);
            }

            if (flagPago == "S") {
                query = query.Where(x => x.dtPagamento != null);
            }

            if (flagPago == "N") {
                query = query.Where(x => x.dtPagamento == null);
            }

            if (flagPago == "A") {
                query = query.Where(x => x.dtPagamento == null && x.dtVencimentoRecebimento < DateTime.Today);
            }

            dtInicio =  dtInicio ?? new DateTime(2000, 1, 1);

            dtFim =  dtFim.HasValue? dtFim?.AddHours(23).AddMinutes(59).AddSeconds(59) : new DateTime(2999, 1, 1);

            if (pesquisarPor == "P") {

                query = query.Where(x => (x.dtPagamento >= dtInicio) && (x.dtPagamento <= dtFim));

            } else if (pesquisarPor == "C") {

                query = query.Where(x => (x.dtCredito >= dtInicio) && (x.dtCredito <= dtFim));

            } else if (pesquisarPor == "PC") {

                query = query.Where(x => (x.dtPrevisaoCredito >= dtInicio) && (x.dtPrevisaoCredito <= dtFim));

            } else {

                query = query.Where(x => (x.dtVencimentoRecebimento >= dtInicio) && (x.dtVencimentoRecebimento <= dtFim));
            }

            if (idMacroConta > 0) {
                query = query.Where(x => x.idMacroConta == idMacroConta);
            }

            if (idContaBancaria > 0) {
                query = query.Where(x => x.idContaBancaria == idContaBancaria);
            }

            return query;
        }

        public IQueryable<TituloReceitaPagamentoResumoVW> listarPagamentoReceitasExcluidas(string descricao, int idCentroCusto, int idTipoReceita, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim, int? idMacroConta, int? idContaBancaria) {

            var query = from TR in this.db.TituloReceitaPagamentoResumoVW
                        .Where(x => x.dtExclusao != null)
                        select TR;

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
                query = query.Where(x => (x.dtExclusao >= dtInicio) && (x.dtExclusao <= dtFim));
            } else if (pesquisarPor == "C") {
                query = query.Where(x => (x.dtCredito >= dtInicio) && (x.dtCredito <= dtFim));
            } else {
                query = query.Where(x => (x.dtVencimentoRecebimento >= dtInicio) && (x.dtVencimentoRecebimento <= dtFim));
            }

            return query;
        }

    }
}
