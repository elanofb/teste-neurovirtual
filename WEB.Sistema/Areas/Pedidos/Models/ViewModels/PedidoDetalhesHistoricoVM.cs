using System.Collections.Generic;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoDetalhesHistoricoVM {
        
        public int idPedido { get; set; }

        public List<PedidoHistorico> listaOcorrencias { get; set; }

	}

}