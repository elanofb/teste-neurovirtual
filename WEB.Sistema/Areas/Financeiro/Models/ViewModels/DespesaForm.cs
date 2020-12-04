using System.Collections.Generic;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class DespesaForm {

        public TituloDespesa TituloDespesa { get; set; }

        public IList<TituloDespesaPagamento> listaTituloDespesaPagamento { get; set; }

        public string urlRetorno { get; set; }
    }
}