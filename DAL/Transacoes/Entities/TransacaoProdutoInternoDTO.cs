
using System.Collections.Generic;
using DAL.Produtos;

namespace DAL.Transacoes {

    public class TransacaoProdutoInterno {

        public decimal valorPagamento { get; set; }

        public decimal percentualTotalDistribuicao { get; set; }
        
        public decimal percentualTotalInterno { get; set; }

        public decimal valorDistribuicao { get; set; }

        public decimal valorInterno { get; set; }
        
        public List<ProdutoRedeConfiguracao> listaComissoes { get; set; }
        
    }

}
