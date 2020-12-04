using DAL.Pedidos;
using FluentValidation.Attributes;

namespace WEB.Areas.Pedidos.ViewModels{
    
    [Validator(typeof(PedidoDetalhesFreteFormValidator))]
    public class PedidoDetalhesFreteForm {
        
        public PedidoEntrega PedidoEntrega { get; set; }
               
	}

}