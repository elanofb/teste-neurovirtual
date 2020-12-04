using System.Collections.Generic;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ReceitaForm {

        public TituloReceita TituloReceita { get; set; }

        public IList<TituloReceitaPagamento> listaTituloReceitaPagamento { get; set; }

        public string urlRetorno { get; set; }
    }
}