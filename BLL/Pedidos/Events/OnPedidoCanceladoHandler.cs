using BLL.Core.Events;
using BLL.Financeiro;
using DAL.Financeiro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BLL.Pedidos {

    public class OnPedidoCanceladoHandler : IHandler<object> {

        //Propriedades
	    private ITituloReceitaExclusaoBL OTituloReceitaExclusaoBL => new TituloReceitaExclusaoBL();
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => new PedidoHistoricoBL();

		//
		public void execute(object source) {

            try {

                var listaParametros = source as List<object>;

                var idPedido = Convert.ToInt32(listaParametros[0]);
            
		        var observacoes = listaParametros[1].ToString();

                this.cancelarTitulos(idPedido);

                this.registrarOcorrencia(idPedido, observacoes);

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao executar o evento OnPedidoCanceladoHandler.");

            }

		}

		//Cancelamento de titulos de receita do pedido
		private void cancelarTitulos(int idPedido) {

            try {

                this.OTituloReceitaExclusaoBL.excluir(TipoReceitaConst.PEDIDO, idPedido, "Pedido Cancelado");

            } catch (SqlException ex) {

	            UtilLog.saveError(ex, $"Erro ao cancelar titulo receita do pedido { idPedido } ");

            } catch (Exception ex) {

	            UtilLog.saveError(ex, $"Erro ao cancelar titulo receita do pedido { idPedido }");
	        }

		}

        // Gerar ocorrência de cancelamento do pedido
        private void registrarOcorrencia(int id, string observacoes) {

            this.OPedidoOcorrenciaBL.criarOcorrenciaCancelado(id, observacoes);

        }
        
	}
}
