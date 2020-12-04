using BLL.Core.Events;
using System;
using DAL.Pedidos;

namespace BLL.Pedidos {

	public class OnProdutoAdicionadoHandler : IHandler<object> {

        //Propriedades
        private IPedidoFreteBL OPedidoFreteBL => new PedidoFreteBL();

	    private IPedidoRecalculoBL OPedidoRecalculoBL => new PedidoRecalculoBL();

	    private IPedidoHistoricoBL OPedidoOcorrenciaBL => new PedidoHistoricoBL();

	    private PedidoProduto OPedidoProduto { get; set; }

		//
		public void execute(object source) {

		    this.OPedidoProduto = source as PedidoProduto;

            this.recalcularValorPedido();

            this.recalcularFrete();
            
            this.registrarOcorrencia();

		}


        // Recalcular valor total do pedido
	    private void recalcularValorPedido() {

	        try {

    			this.OPedidoRecalculoBL.recalcularValorPedido(this.OPedidoProduto.idPedido);

	        } catch (Exception ex) {

	            UtilLog.saveError(ex, $"OnProdutoAdicionadoHandler: Erro ao recalcular o valor total do pedido do pedido { this.OPedidoProduto.idPedido }");

	        }
	        
	    }

	    // Recalcular frete após alterar endereço de entrega
	    private void recalcularFrete() {

	        try {

	            this.OPedidoFreteBL.recalcularFrete(this.OPedidoProduto.idPedido);

	        } catch (Exception ex) {

	            UtilLog.saveError(ex, $"OnProdutoAdicionadoHandler: Erro ao recalcular o frete do pedido { this.OPedidoProduto.idPedido }");

	        }
	        
	    }

	    // Gerar ocorrência de atendimento do pedido
	    private void registrarOcorrencia() {
            
	        try {

	            var observacoes = $"Item adicionado manualmente: #{ this.OPedidoProduto.idProduto } - { this.OPedidoProduto.nomeProduto } | Qtde.: { this.OPedidoProduto.qtde }";

	            int idOcorrencia = TipoOcorrenciaPedidoConst.INCLUSAO_PRODUTO;

	            var OPedidoOcorrencia = this.OPedidoOcorrenciaBL.criarNovaOcorrencia(this.OPedidoProduto.idPedido, idOcorrencia, observacoes);

	            this.OPedidoOcorrenciaBL.salvar(OPedidoOcorrencia);
            
	        } catch (Exception ex) {

	            UtilLog.saveError(ex, $"OnProdutoAdicionadoHandler: Erro ao adicionar um item do pedido { this.OPedidoProduto.idPedido }");

	        }
	    }
        
	}

}
