
namespace DAL.Transacoes {

    public class TransacaoPagamentoDTO {

        public decimal valorPagamento { get; set; }
        
        public decimal percentualDescontoEstabelecimento { get; set; }

        public decimal valorDesconto { get; set; }
        
        public decimal valorCashback { get; set; }
        
        public decimal valorLinkey { get; set; }

        public decimal valorEstabelecimento { get; set; }

        public decimal valorComissaoCorretor { get; set; }
        
        public decimal valorComissaoIndicador1 { get; set; }
        
        public decimal valorComissaoIndicador2 { get; set; }
        
        public decimal valorComissaoIndicador3 { get; set; }
    }

}
