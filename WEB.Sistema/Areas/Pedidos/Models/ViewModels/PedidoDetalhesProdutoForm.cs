using DAL.Pedidos;
using FluentValidation.Attributes;

namespace WEB.Areas.Pedidos.ViewModels{
    
    [Validator(typeof(PedidoDetalhesProdutoFormValidator))]
    public class PedidoDetalhesProdutoForm {
        
        public PedidoProduto OPedidoProduto { get; set; }

	}

}