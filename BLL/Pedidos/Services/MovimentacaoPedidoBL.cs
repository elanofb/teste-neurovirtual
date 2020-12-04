using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class MovimentacaoPedidoBL : DefaultBL, IMovimentacaoPedidoBL {
	    
	    // Atributos Serviço
	    

	    // Propriedades Serviço        
	    

        //Events
	    private EventAggregator onPedidoEmMontagem => OnPedidoEmMontagem.getInstance;
	    private EventAggregator onPedidoPreparadoExpedicao => OnPedidoPreparadoExpedicao.getInstance;
		
		//
		public MovimentacaoPedidoBL() {
            
        }
        
        //
        public bool alterarStatus(List<int> idsPedidos, int idStatusPedido) {
			
	        if (!idsPedidos.Any() || idStatusPedido == 0){
		        return false;
	        }
	        
            db.Pedido.condicoesSeguranca().Where(x => idsPedidos.Contains(x.id))
              .Update(x => new Pedido {
					
                    dtAtendimento = DateTime.Now,
		            idStatusPedido = idStatusPedido,
		            idUsuarioAlteracao = User.id()                    
		            
              });
	        
	        var listaParametros = new List<int>();
	        listaParametros.AddRange(idsPedidos);
			
	        if (idStatusPedido == StatusPedidoConst.EM_MONTAGEM){
		        this.onPedidoEmMontagem.subscribe(new OnPedidoEmMontagemHandler());
		        this.onPedidoEmMontagem.publish(listaParametros as object);
	        }
	        
	        if (idStatusPedido == StatusPedidoConst.AGUARDANDO_EXPEDICAO){
		        this.onPedidoPreparadoExpedicao.subscribe(new OnPedidoPreparadoExpedicaoHandler());
		        this.onPedidoPreparadoExpedicao.publish(listaParametros as object);
	        }
	        	        	        	        		
	        return true;

        }
           
	}

}