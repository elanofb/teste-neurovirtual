
using System.Collections.Generic;

namespace DAL.Financeiro {
    public class ExtratoAnaliticoDTO {
        public int id { get; set; }
        public string nome { get; set; }
        public decimal valorReceita { get; set; }
        public decimal valorDespesa { get; set; }
        public decimal valor { get; set; }
        public string flagReceitaDespesa { get; set; }
        public List<ExtratoDetalhe> listaDetalhe { get; set; }
    }

    public class ExtratoDetalhe {
        public int id { get; set; }
        public string nome { get; set; }
        public decimal valor { get; set; }
        public string flagReceitaDespesa { get; set; }
    }
}
