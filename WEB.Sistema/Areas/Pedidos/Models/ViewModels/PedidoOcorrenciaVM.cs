using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{

	[Validator(typeof(PedidoOcorrenciaValidator))]
	public class PedidoOcorrenciaVM{
        public int id { get; set;}
        public int idPedido { get; set;}
        public int idStatusAtual { get; set;}
        public int idStatusPedido { get; set;}
        public int idOcorrencia { get; set;}
        public string observacoes { get; set;}
        public string nomePessoa { get; set;}
		public List<PedidoHistorico> listaOcorrencias { get; set;}

		public PedidoOcorrenciaVM() { 
			this.listaOcorrencias = new List<PedidoHistorico>();
		}
	}

	//
	internal class PedidoOcorrenciaValidator : AbstractValidator<PedidoOcorrenciaVM> {

		public static readonly int idStatusAtendido = StatusPedidoConst.ATENDIDO;
		public static readonly int idStatusExpedido = StatusPedidoConst.EXPEDIDO;
		public static readonly int idStatusPago = StatusPedidoConst.PAGO;
		public static readonly int idStatusCancelado = StatusPedidoConst.CANCELADO;
		
		//
		public PedidoOcorrenciaValidator() {

			RuleFor(x => x.idPedido)
				.NotEmpty().WithMessage("Informe o número do pedido.");

			RuleFor(x => x.idStatusPedido)
				.NotEmpty().WithMessage("Informe o status do pedido.");


			RuleFor(x => x.observacoes)
				.NotEmpty().When(x => x.idStatusPedido == StatusPedidoConst.CANCELADO).WithMessage("Informe o motivo do cancelamento do pedido.");
		 }


	}
}