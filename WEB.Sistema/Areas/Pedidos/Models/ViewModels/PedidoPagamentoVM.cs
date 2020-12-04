using System.Collections.Generic;
using DAL.Financeiro;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{

	public class PedidoPagamentoVM {

        public Pedido Pedido { get; set;}

        public List<TituloReceitaPagamento> listaPagamentos { get; set; }

        public int qtdeParcelas { get; set; }

        public bool flagTemPagamento { get; set; }

        //
	    public PedidoPagamentoVM() {
	        this.listaPagamentos = new List<TituloReceitaPagamento>();
	    }
	}

}