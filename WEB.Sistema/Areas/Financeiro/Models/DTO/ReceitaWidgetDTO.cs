
using System;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ReceitaWidgetDTO {

        public int idTituloReceitaPagamento { get; set; }
        public int idTituloReceita { get; set; }
        public byte idTipoReceita { get; set; }
        public int? idParcelaPrincipal { get; set; }
        public byte qtdeParcelas { get; set; }
        public string descricaoTipoReceita { get; set; }
        public byte? idStatusPagamento { get; set; }
        public string descricaoStatusPagamento { get; set; }
        public string descricaoMeioPagamento { get; set; }
        public string descricaoTitulo { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime? dtPagamento { get; set; }
        public string nomePessoa { get; set; }
        public decimal valorOriginal { get; set; }
        public decimal? valorRecebido { get; set; }
        public decimal valorDescontoAntecipacao { get; set; }
        public decimal valorDescontoCupom { get; set; }
    }

}