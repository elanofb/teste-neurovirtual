using System.Collections.Generic;
using System.Web.Mvc;
using DAL.Pedidos;

namespace WEB.Helpers{

    public class StatusPedidoHelper{
        
        // Atributos
        public static StatusPedidoHelper _intance;

        // Propriedades
        public static StatusPedidoHelper getInstance => _intance = _intance ?? new StatusPedidoHelper();

        //
        public MultiSelectList selectMultiList(List<int> selected){

            var list = new[] { 

                new { value = StatusPedidoConst.EM_ABERTO, text = "EM ABERTO" },
                new { value = StatusPedidoConst.AGUARDANDO_PAGAMENTO, text = "AGUARDANDO PAGAMENTO" },
                new { value = StatusPedidoConst.PAGO, text = "PAGO" },
                new { value = StatusPedidoConst.EM_PREPARACAO, text = "EM MONTAGEM" },
                new { value = StatusPedidoConst.ATENDIDO, text = "ATENDIDO" },
                new { value = StatusPedidoConst.EXPEDIDO, text = "EXPEDIDO" },
                new { value = StatusPedidoConst.FINALIZADO, text = "FINALIZADO" },
                new { value = StatusPedidoConst.CANCELADO, text = "CANCELADO" },
                new { value = StatusPedidoConst.PENDENTE, text = "PENDENTE" }

            };

            return new MultiSelectList(list, "value", "text", selected);

        }

        public MultiSelectList selectMultiListProducao(List<int> selected) {

            var list = new[] {

                new { value = StatusPedidoConst.EM_ABERTO, text = "EM ABERTO" },
                new { value = StatusPedidoConst.AGUARDANDO_PAGAMENTO, text = "AGUARDANDO PAGAMENTO" },
                new { value = StatusPedidoConst.PAGO, text = "PAGO" }
            };

            return new MultiSelectList(list, "value", "text", selected);

        }

        //
        public static SelectList getComboWorkFlow(int idStatusAtual){

            object idEmAberto	= new{value =  StatusPedidoConst.EM_ABERTO, text = "EM ABERTO"} ;
			object idAguardandoPagamento	= new{value =  StatusPedidoConst.AGUARDANDO_PAGAMENTO, text = "AGUARDANDO PAGAMENTO"} ;
			object idPago					= new{value =  StatusPedidoConst.PAGO, text = "PAGO"} ;
			object idEmMontagem				= new{value =  StatusPedidoConst.EM_PREPARACAO, text = "EM MONTAGEM"};
			object idAtendido				= new{value =  StatusPedidoConst.ATENDIDO, text = "ATENDIDO"} ;
			object idExpedido				= new{value =  StatusPedidoConst.EXPEDIDO, text = "EXPEDIDO"};
			object idFinalizado				= new{value =  StatusPedidoConst.FINALIZADO, text = "FINALIZADO" };
			object idCancelado				= new{value =  StatusPedidoConst.CANCELADO, text = "CANCELADO"};
			object idPendente				= new{value =  StatusPedidoConst.PENDENTE, text = "PENDENTE"} ;
			object idEmAndamento			= new{value =  StatusPedidoConst.EM_ANDAMENTO, text = "EM ANDAMENTO"} ;
			var list = new object[]{};

			if(idStatusAtual == StatusPedidoConst.AGUARDANDO_PAGAMENTO){
				list[0] = idPago;
				list[1] = idCancelado;
			}else if(idStatusAtual == StatusPedidoConst.PAGO){
				list[0] = idAtendido;
				list[1] = idCancelado;
			}else if(idStatusAtual == StatusPedidoConst.ATENDIDO){
				list[0] = idEmMontagem;
				list[1] = idExpedido;
				list[2] = idCancelado;
			}else if(idStatusAtual == StatusPedidoConst.EM_PREPARACAO){
				list[0] = idExpedido;
				list[1] = idCancelado;
			}else if(idStatusAtual == StatusPedidoConst.EXPEDIDO){
				list[0] = idFinalizado;
				list[1] = idCancelado;
			}else if(idStatusAtual == StatusPedidoConst.EM_ANDAMENTO){
				list[0] = idAguardandoPagamento;
				list[1] = idPago;
				list[2] = idCancelado;
			}

            return new SelectList(list, "value", "text", 0);
        }

        //
        public static SelectList getComboStatusPedido(int selected){
            var list = new[] { 
                new{value = StatusPedidoConst.EM_ABERTO, text = "EM ABERTO"},
                new{value = StatusPedidoConst.AGUARDANDO_PAGAMENTO, text = "AGUARDANDO PAGAMENTO"},
                new{value = StatusPedidoConst.PAGO, text = "PAGO"},
                new{value = StatusPedidoConst.EM_PREPARACAO, text = "EM MONTAGEM"},
                new{value = StatusPedidoConst.ATENDIDO, text = "ATENDIDO"},
                new{value = StatusPedidoConst.EXPEDIDO, text = "EXPEDIDO"},
                new{value = StatusPedidoConst.FINALIZADO, text = "FINALIZADO"},
                new{value = StatusPedidoConst.CANCELADO, text = "CANCELADO"},
                new{value = StatusPedidoConst.PENDENTE, text = "PENDENTE"},
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList getComboStatusPedidoNaoFinalizadores(int selected){
            var list = new[] { 
                new{ value = StatusPedidoConst.EM_ABERTO, text = "EM ABERTO" },
                new{ value = StatusPedidoConst.AGUARDANDO_PAGAMENTO, text = "AGUARDANDO PAGAMENTO" },
                new{ value = StatusPedidoConst.PAGO, text = "PAGO" },
                new{ value = StatusPedidoConst.EM_PREPARACAO, text = "EM MONTAGEM" },
                new{ value = StatusPedidoConst.ATENDIDO, text = "ATENDIDO" },
            };
            return new SelectList(list, "value", "text", selected);
        }
    }
}