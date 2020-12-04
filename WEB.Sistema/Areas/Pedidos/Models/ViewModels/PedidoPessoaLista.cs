using System.Collections.Generic;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{

	public class PedidoPessoaLista{

        public int idPessoa { get; set; }

        public List<Pedido> listaPedidos { get; set; }

        //Construtor
	    public PedidoPessoaLista() {
	        this.listaPedidos = new List<Pedido>();
	    }
	}

}