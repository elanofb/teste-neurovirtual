using System;
using DAL.Financeiro;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.Financeiro.Extensions {

    public static class TituloPagamentoResumoDTOExtensions {
        
        //Borda
        public static string exibirBordaStatus(this TituloPagamentoResumoDTO OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "border-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "border-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "border-red" : "border-yellow"));

            return descricaoAtivo;
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeStatus(this TituloPagamentoResumoDTO OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "fa-times-circle";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "fa-check" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "fa-times-circle" : "fa-exclamation"));

            return descricaoAtivo;
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseStatus(this TituloPagamentoResumoDTO OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "text-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "text-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "text-red" : "text-yellow"));

            return descricaoAtivo;
        }

    }
    
}