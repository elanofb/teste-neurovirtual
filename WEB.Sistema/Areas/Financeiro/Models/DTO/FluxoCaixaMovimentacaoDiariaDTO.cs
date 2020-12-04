using System;
using System.Collections.Generic;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class FluxoCaixaMovimentacaoDiariaDTO {

        public DateTime dtReferencia { get; set; }

        public int idContaBancaria { get; set; }
        
        public decimal valorTotalEntrada { get; set; }

        public decimal valorTotalSaida { get; set; }

        public decimal saldoDia { get; set; }

        public decimal saldoAcumulado { get; set; }

        public List<ReceitaDespesaVW> listaPagamentosMovimentacao { get; set; }

    }

}