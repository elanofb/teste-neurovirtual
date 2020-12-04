using System;
using System.Linq;
using BLL.Services;
using DAL.Pedidos;
using DAL.Compras;
using DAL.Pessoas;

namespace BLL.Compras {

	public class CarrinhoBL : DefaultBL, ICarrinhoBL {

		//Atributos

		//Propriedades


	    //Carregar um item do carrinho pelo ID
		public CarrinhoItem carregarItem(int idItem) {
			var query = from Car in db.CarrinhoItem
						where
							Car.id == idItem
						select
							Car;

			var Retorno = query.FirstOrDefault();

			return Retorno;
		}


		//Atualizacao das quantidades do carrinho de compras
		public UtilRetorno alterarQuantidade(int idItem, byte qtde) {

			var OCarrinhoItem = this.carregarItem(idItem);

			if (OCarrinhoItem == null) {
				return UtilRetorno.newInstance(true, "Não foi possível atualizar o item informado.");
			}

			OCarrinhoItem.qtde = qtde;

			this.db.SaveChanges();

			return UtilRetorno.newInstance(false, "A quantidade foi alterada com sucesso.");
		}

	
		////Criar o pedido de acordo com as compras do usuario
		//public async Task<Pedido> criarPedido(Pedido OPedido) {

		//	var OCarrinhoResumo = this.OCarrinhoResumoBL.carregarExistente(OPedido.idPessoa, OPedido.idSessao);

		//	OCarrinhoResumo.listaItens = this.listar(OPedido.idPessoa, OPedido.idSessao, false).ToList();
			
		//	OPedido.listaProdutos = new List<PedidoProduto>();

		//	foreach(var ItemCarrinho in OCarrinhoResumo.listaItens){
				
		//		Produto OProduto = this.OProdutoBL.carregar(ItemCarrinho.idProduto.toInt());
				
		//		PedidoProduto OPedidoProduto = new PedidoProduto();

		//		OPedidoProduto.idProduto = OProduto.id;

		//		OPedidoProduto.qtde = ItemCarrinho.qtde;
				
		//		OPedidoProduto.valorItem = ItemCarrinho.valorUnitario;
				
		//		OPedidoProduto.peso = ItemCarrinho.pesoUnitario;
				
		//		OPedidoProduto.nomeProduto = OProduto.nome;

		//		OPedido.listaProdutos.Add(OPedidoProduto);
		//	}


		//	PedidoEntrega OPedidoEntrega = new PedidoEntrega();

		//	OPedidoEntrega.cep = OCarrinhoResumo.cepDestino;

		//	OPedidoEntrega.idTipoFrete = OCarrinhoResumo.idTipoFrete;

		//	CepBrasil CepBrasil = await this.OCepBrasilBL.buscarEndereco(OCarrinhoResumo.cepDestino);

		//	if (CepBrasil != null) {

		//		OPedidoEntrega.logradouro = String.Concat(CepBrasil.tipoLogradouro, " ", CepBrasil.logradouro);

		//		OPedidoEntrega.bairro = CepBrasil.bairroIni;

		//		OPedidoEntrega.nomeCidade = CepBrasil.nomeCidade;

		//		OPedidoEntrega.idCidade = CepBrasil.idCidade;

		//		OPedidoEntrega.idEstado = CepBrasil.idEstado;

		//		OPedidoEntrega.idPais = "BRA";
		//	}

		//	OPedidoEntrega.setDefaultInsertValues();

		//	OPedido.listaPedidoEntrega.Add(OPedidoEntrega);

		//	OPedido.valorFrete = OCarrinhoResumo.valorFrete;

		//	OPedido.valorProdutos = OCarrinhoResumo.valorItens();

		//	//Preencher dados da pessoa
		//	if (OPedido.idPessoa > 0) {
		//		this.preencherDadosPessoa(ref OPedido);
		//	}

		//	this.OPedidoBL.salvar(OPedido);

		//	return OPedido;
		//}

		//
		private void preencherDadosPessoa(ref Pedido OPedido) {

			int idPessoa = UtilNumber.toInt32(OPedido.idPessoa);

			var OPessoa = db.Pessoa.FirstOrDefault(x => x.id == idPessoa);

			if (OPessoa == null) {
				return;
			}

			OPedido.nomePessoa = OPessoa.nome;

			OPedido.cpf = OPessoa.nroDocumento;

			OPedido.rg = OPessoa.rg;

			OPedido.email = OPessoa.emailPrincipal();

			OPedido.telPrincipal = String.Concat(OPessoa.dddTelPrincipal, OPessoa.nroTelPrincipal);

			OPedido.telSecundario = String.Concat(OPessoa.dddTelSecundario, OPessoa.nroTelSecundario);

		}
	}
}
