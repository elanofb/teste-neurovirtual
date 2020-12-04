using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.Core.Events;
using BLL.Produtos;
using BLL.Services;
using DAL.Pedidos;
using DAL.Produtos;

namespace BLL.Pedidos {

	public class PedidoProdutoOperacaoBL : DefaultBL, IPedidoProdutoOperacaoBL {

	    //Atributos
	    private IAssociadoBL _IAssociadoBL;
	    private IProdutoBL _IProdutoBL;
	    private IPedidoBL _IPedidoBL;
	    private IPedidoProdutoBL _IPedidoProdutoBL;

        // Propriedades
	    private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
	    private IProdutoBL OProdutoBL => _IProdutoBL = _IProdutoBL ?? new ProdutoBL();
	    private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
	    private IPedidoProdutoBL OPedidoProdutoBL => _IPedidoProdutoBL = _IPedidoProdutoBL ?? new PedidoProdutoBL();

        // Events
	    private EventAggregator onProdutoRemovido => OnProdutoRemovido.getInstance;
	    private EventAggregator onProdutoAdicionado => OnProdutoAdicionado.getInstance;

		//
		public PedidoProdutoOperacaoBL() {

		}

	    //
	    public void adicionar(PedidoProduto OPedidoProduto) {
            
	        var OPedido = this.OPedidoBL.carregar(OPedidoProduto.idPedido);
            
	        var listaPedidoProduto = OPedido?.listaProdutos?.Where(x => x.flagExcluido == "N");
            
	        var dbPedidoProduto = listaPedidoProduto?.FirstOrDefault(x => x.idProduto == OPedidoProduto.idProduto);
            
	        if (dbPedidoProduto != null) {
                
	            dbPedidoProduto.qtde += OPedidoProduto.qtde;

	            this.OPedidoProdutoBL.salvar(dbPedidoProduto);
	            
	            OPedidoProduto.nomeProduto = dbPedidoProduto.nomeProduto;

	            // Disparo de evento
	            this.onProdutoAdicionado.subscribe(new OnProdutoAdicionadoHandler());

                this.onProdutoAdicionado.publish(OPedidoProduto as object);

	            return;

	        }
            
	        var OProduto = this.OProdutoBL.carregar(OPedidoProduto.idProduto);

	        OPedidoProduto.idPedido = OPedido.id;

	        OPedidoProduto.idProduto = OProduto.id;

	        OPedidoProduto.nomeProduto = OProduto.nome;

	        // Definir valor de acordo com as regras configuradas
	        OPedidoProduto.valorItem = OPedidoProduto.valorItem;

	        OPedidoProduto.Pedido = OPedido;

	        this.definirValor(OPedidoProduto, OProduto);
            
	        OPedidoProduto.peso = OProduto.peso;

	        OPedidoProduto.qtde = OPedidoProduto.qtde;

	        OPedidoProduto.flagCalcularFrete = OProduto.flagCalcularFrete;
            
	        this.OPedidoProdutoBL.salvar(OPedidoProduto);

            // Disparo de evento
	        this.onProdutoAdicionado.subscribe(new OnProdutoAdicionadoHandler());

	        this.onProdutoAdicionado.publish(OPedidoProduto as object);
            
	    }

        //
	    private void definirValor(PedidoProduto OPedidoProduto, Produto OProduto) {

	        // Se o produto for cortesia, zerar o valor
	        if (OProduto.flagCortesia == true) {

	            OPedidoProduto.valorItem = 0;

	            return;

	        }

	        // Se o produto não for configurável, considerar o valor cadastrado no produto
	        if (OProduto.flagValorConfiguravel != true) {

	            // Se o comprador for um associado, verificar se há descontos
	            var OAssociado = this.OAssociadoBL.carregarAssociadoPessoa(OPedidoProduto.Pedido.idPessoa.toInt());

	            if (OAssociado != null) {

	                OPedidoProduto.valorItem = OProduto.getValorComDescontoAssociado();

	                return;

	            }

	            // Se não houver desconto, retornar o valor original
	            OPedidoProduto.valorItem = OProduto.valor;
                
	        }
            
	    }

	    //
	    public UtilRetorno excluir(int id) {

	        var ORetorno = UtilRetorno.newInstance(false);
            
	        var OPedidoProduto = db.PedidoProduto.FirstOrDefault(x => x.id == id && x.flagExcluido == "N");

	        if (OPedidoProduto == null) {
	            ORetorno.flagError = true;
	            ORetorno.listaErros.Add("O item informado não foi encontrado.");
	            return ORetorno;
	        }

	        var dbPedido = db.Pedido.FirstOrDefault(x => x.id == OPedidoProduto.idPedido);

	        if (dbPedido?.idStatusPedido == StatusPedidoConst.CANCELADO) {
	            ORetorno.flagError = true;
	            ORetorno.listaErros.Add("Você não pode remover nenhum item do pedido, pois está cancelado.");
	            return ORetorno;
	        }

	        if (dbPedido?.idStatusPedido == StatusPedidoConst.FINALIZADO) {
	            ORetorno.flagError = true;
	            ORetorno.listaErros.Add("Você não pode remover nenhum item do pedido, pois está finalizado.");
	            return ORetorno;
	        }

	        var listaProdutosPedido = dbPedido?.listaProdutos.Where(x => x.flagExcluido == "N").ToList();

	        // Se não existir nenhum produto além do que está sendo removido, retornar error informando que o pedido 
	        // não pode ficar sem produtos 
	        if (listaProdutosPedido?.Any(x => x.id != OPedidoProduto.id) == false) {
	            ORetorno.flagError = true;
	            ORetorno.listaErros.Add("Você não pode remover todos o produtos do pedido.");
	            return ORetorno;
	        }
            
	        OPedidoProduto.flagExcluido = "S";

	        db.SaveChanges();
            
	        this.onProdutoRemovido.subscribe(new OnProdutoRemovidoHandler());

	        this.onProdutoRemovido.publish(OPedidoProduto as object);
            
	        return ORetorno;

	    }

	}
}