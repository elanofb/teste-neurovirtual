using BLL.Core.Events;
using DAL.Pedidos;
using BLL.CuponsDesconto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BLL.AssociadosInstitucional.Emails;
using BLL.Pedidos.Emails;
using BLL.Services;
using DAL.Pessoas;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

	public class PedidoCadastradoHandler : DefaultBL, IHandler<object> {

        //Propriedades
	    private IPedidoAcaoFaturamentoBL OPedidoFaturamentoBL => new PedidoAcaoFaturamentoBL();
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => new PedidoHistoricoBL();
	    private ICupomDescontoBL OCupomDescontoBL => new CupomDescontoBL();

        //
        public void execute(object source) {

            Pedido OPedido = (source as Pedido);

            this.gerarOcorrencia(OPedido);
            
            this.registrarUsoCupom(OPedido);

            if(OPedido?.getValorTotal() > 0) {

                this.faturarPedido(OPedido);
            }

	        // Pedido gratuito (e sem frete)
	        if (OPedido?.getValorTotal() == 0) {
		        
		        this.registrarPagamentoPedido(OPedido);
		        
		        this.enviarEmailNovoPedido(OPedido);
		        
	        }

        }


        //gerar a ocorrencia apos criacao do pedido
	    private void gerarOcorrencia(Pedido OPedido) {

	        try {
		        
    			this.OPedidoOcorrenciaBL.criarOcorrenciaPedidoCriado(OPedido.id);

	        } catch (Exception ex) {
	            
	            UtilLog.saveError(ex, $"Erro ao executar o metodo gerarOcorrencia() no manipulador PedidoCadastradoHandler. Pedido: " + OPedido.id);		        
	        }
	        
	    }
        

        //Salvar a utilizacao do cupom de desconto
	    private void registrarUsoCupom(Pedido OPedido) {
            try { 

                if (OPedido.idCupomDesconto.HasValue) {

                    OCupomDescontoBL.registrarUso(Convert.ToInt32(OPedido.idCupomDesconto));
                }

            } catch (Exception ex) {
	            
	            UtilLog.saveError(ex, $"Erro ao executar o metodo registrarUsoCupom() no manipulador PedidoCadastradoHandler. Pedido: " + OPedido.id);   
	        }
		    
	    }

        // Faturar o pedido
        private void faturarPedido(Pedido OPedido) {

            try { 

                this.OPedidoFaturamentoBL.faturarPedido(OPedido.id);

            } catch (Exception ex) {

	            UtilLog.saveError(ex, $"Erro ao executar o metodo faturarPedido() no manipulador PedidoCadastradoHandler. Pedido: " + OPedido.id);
            }

        }
		
		// Se for um pedido gratuito, atualizar o status do pedido para pago
		private void registrarPagamentoPedido(Pedido OPedido) {
			
			db.Pedido.Where(x => x.id == OPedido.id)
			  .Update(x => new Pedido {
				
				  dtFaturamento = DateTime.Now,
				
				  dtQuitacao = DateTime.Now,

				  idStatusPedido = StatusPedidoConst.PAGO	
					
			  });
			
		}

		// Enviar e-mail após a criação do pedido
		private void enviarEmailNovoPedido(Pedido OPedido) {

			try {

				IEnvioNovoPedido EnvioEmail = EnvioNovoPedido.factory(OPedido.idOrganizacao.toInt(), new List<string> { OPedido.email }, null);

				EnvioEmail.enviar(OPedido);

			} catch (Exception ex) {
				UtilLog.saveError(ex, $"Erro ao enviar e-mail do pedido {OPedido.id}");
			}

		}

    }
}
