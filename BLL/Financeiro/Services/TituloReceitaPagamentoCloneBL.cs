using System;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoCloneBL : DefaultBL, ITituloReceitaPagamentoCloneBL {

        //Carregamento de registro pelo ID
        public bool salvarClone(TituloReceitaPagamento TituloReceitaPagamento) {

            var OTituloReceitaPagamento = new TituloReceitaPagamento();

            OTituloReceitaPagamento.idTituloReceita = TituloReceitaPagamento.idTituloReceita;
            OTituloReceitaPagamento.idStatusPagamento = TituloReceitaPagamento.idStatusPagamento;
            OTituloReceitaPagamento.idContaBancaria = TituloReceitaPagamento.idContaBancaria;
            OTituloReceitaPagamento.idMacroConta = TituloReceitaPagamento.idMacroConta;
            OTituloReceitaPagamento.idCentroCusto = TituloReceitaPagamento.idCentroCusto;
            OTituloReceitaPagamento.idCategoria = TituloReceitaPagamento.idCategoria;
            OTituloReceitaPagamento.descricaoParcela = TituloReceitaPagamento.descricaoParcela;
            OTituloReceitaPagamento.valorOriginal = TituloReceitaPagamento.valorOriginal;
            OTituloReceitaPagamento.valorDesconto = TituloReceitaPagamento.valorDesconto;
            OTituloReceitaPagamento.valorJuros = TituloReceitaPagamento.valorJuros;
            OTituloReceitaPagamento.valorTarifasBancarias = TituloReceitaPagamento.valorTarifasBancarias;
            OTituloReceitaPagamento.valorTarifasTransacao = TituloReceitaPagamento.valorTarifasTransacao;
            OTituloReceitaPagamento.valorOutrasTarifas = TituloReceitaPagamento.valorOutrasTarifas;
            OTituloReceitaPagamento.motivoDesconto = TituloReceitaPagamento.motivoDesconto;
            OTituloReceitaPagamento.dtVencimento = TituloReceitaPagamento.dtVencimento;
            OTituloReceitaPagamento.dtVencimentoOriginal = TituloReceitaPagamento.dtVencimento;
            OTituloReceitaPagamento.dtCompetencia = TituloReceitaPagamento.dtCompetencia;
            OTituloReceitaPagamento.mesCompetencia = TituloReceitaPagamento.mesCompetencia;
            OTituloReceitaPagamento.anoCompetencia = TituloReceitaPagamento.anoCompetencia;

            OTituloReceitaPagamento.setDefaultInsertValues();

            db.TituloReceitaPagamento.Add(OTituloReceitaPagamento);
            db.SaveChanges();

            return OTituloReceitaPagamento.id > 0;
        }
    }
}