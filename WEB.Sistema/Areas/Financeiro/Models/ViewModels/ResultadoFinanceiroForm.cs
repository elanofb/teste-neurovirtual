using System;
using System.Collections.Generic;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ResultadoFinanceiroForm {
        
        public string tipoBuscaPeriodo { get; set; }

        public DateTime? dtInicioPeriodo { get; set; }

        public DateTime? dtFimPeriodo { get; set; }

        public List<int?> idsCentroCusto { get; set; }

        public List<int?> idsMacroConta { get; set; }

        public List<int?> idsSubConta { get; set; }

        public string flagTipoTitulo { get; set; }

        public string tipoResultado { get; set; }


        // 
        public ResultadoFinanceiroForm() {

            this.idsCentroCusto = new List<int?>();

            this.idsMacroConta = new List<int?>();

            this.idsSubConta = new List<int?>();

        }

    }
}
