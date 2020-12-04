using FluentValidation.Attributes;

namespace WEB.Areas.Pedidos.ViewModels{
    
    [Validator(typeof(PedidoAcaoCancelamentoFormValidator))]
    public class PedidoAcaoCancelamentoForm {
        
        public int id { get; set; }

        public string motivoCancelamento { get; set; }
               
	}

}