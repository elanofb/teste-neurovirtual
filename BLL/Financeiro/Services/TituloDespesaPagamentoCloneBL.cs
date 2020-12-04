using System;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class TituloDespesaPagamentoCloneBL : DefaultBL, ITituloDespesaPagamentoCloneBL {

        //Carregamento de registro pelo ID
        public bool salvarClone(TituloDespesaPagamento TituloDespesaPagamento) {

            var OTituloDespesaPagamento = new TituloDespesaPagamento();

            OTituloDespesaPagamento.idTituloDespesa = TituloDespesaPagamento.idTituloDespesa;
            OTituloDespesaPagamento.idStatusPagamento = TituloDespesaPagamento.idStatusPagamento;
            OTituloDespesaPagamento.idMacroConta = TituloDespesaPagamento.idMacroConta;
            OTituloDespesaPagamento.idCentroCusto = TituloDespesaPagamento.idCentroCusto;
            OTituloDespesaPagamento.idCategoria = TituloDespesaPagamento.idCategoria;
            OTituloDespesaPagamento.descParcela = TituloDespesaPagamento.descParcela;
            OTituloDespesaPagamento.valorOriginal = TituloDespesaPagamento.valorOriginal;
            OTituloDespesaPagamento.dtVencimento = TituloDespesaPagamento.dtVencimento;
            OTituloDespesaPagamento.dtCompetencia = TituloDespesaPagamento.dtCompetencia;
            OTituloDespesaPagamento.mesCompetencia = TituloDespesaPagamento.mesCompetencia;
            OTituloDespesaPagamento.anoCompetencia = TituloDespesaPagamento.anoCompetencia;

            OTituloDespesaPagamento.setDefaultInsertValues();

            db.TituloDespesaPagamento.Add(OTituloDespesaPagamento);
            db.SaveChanges();

            return OTituloDespesaPagamento.id > 0;
        }
    }
}