using System;
using System.Collections.Generic;

namespace DAL.Pedidos.DTO {

    public class PedidoGeralDTO {
        
        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int? idPessoa { get; set; }

        public string nomePessoa { get; set; }

        public string cpf { get; set; }

        public string rg { get; set; }

        public string email { get; set; }

        public string telPrincipal { get; set; }

        public string telSecundario { get; set; }

        public decimal valorProdutos { get; set; }

        public decimal? valorFrete { get; set; }

        public bool? flagFreteGratis { get; set; }

        public decimal? valorDesconto { get; set; }

        public decimal? valorDescontoCupom { get; set; }

        public int idStatusPedido { get; set; }

        public string descricaoStatusPedido { get; set; }

        public List<PedidoProduto> listaProdutos { get; set; }
        
        public List<PedidoEntrega> listaEntregas { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtQuitacao { get; set; }

        public DateTime? dtAtendimento { get; set; }

        public DateTime? dtPreparacao { get; set; }

        public DateTime? dtExpedicao { get; set; }

        public DateTime? dtCancelamento { get; set; }

        public DateTime? dtFinalizado { get; set; }

        public bool flagFaturamentoCadastro { get; set; }
        
        public bool? flagPagamentoNaEntrega { get; set; }

        public DateTime? dtVencimento { get; set; }

        public DateTime? dtFaturamento { get; set; }

        public int? idUsuarioCadastro { get; set; }
    
        public string nomeUsuarioCadastro { get; set; }
    
        public int? idUsuarioAlteracao { get; set; }
    
        public byte? idTipoCadastro { get; set; }
    
        public string ativo { get; set; }
        
        
        public decimal getValorTotal() {

            decimal vlFrete = this.valorFrete ?? 0;
            decimal valorTotal = Decimal.Add(this.valorProdutos, vlFrete);

            valorTotal = Decimal.Subtract(valorTotal, UtilNumber.toDecimal(this.valorDesconto));

            valorTotal = valorTotal > 0 ? valorTotal : 0;

            return valorTotal;
        }
    }
}
