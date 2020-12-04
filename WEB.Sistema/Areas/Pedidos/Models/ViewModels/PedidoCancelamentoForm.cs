using FluentValidation.Attributes;

namespace WEB.Areas.Pedidos.ViewModels{
    
    [Validator(typeof(PedidoCancelamentoFormValidator))]
    public class PedidoCancelamentoForm {
        
        public int[] ids { get; set; }

        public string motivoCancelamento { get; set; }
               
	}

}