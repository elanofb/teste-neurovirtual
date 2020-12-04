using System;
using System.Collections.Generic;
using BLL.AssociadosContribuicoes;
using BLL.Checkout;
using BLL.Financeiro;
using BLL.Pessoas;
using DAL.AssociadosContribuicoes;
using DAL.Checkout;
using DAL.Financeiro;
using DAL.Pessoas;

namespace BLL.Anuidades {

	public class CheckoutMensalidadeBL :CheckoutCompraBL {

        //Atributos
		private ITituloReceitaBL _TituloReceitaMensalidadeBL;

		//Propriedades
        public ITituloReceitaBL OTituloReceitaMensalidadeBL { get{ return _TituloReceitaMensalidadeBL = _TituloReceitaMensalidadeBL ?? new TituloReceitaMensalidadeBL();  }}


		//Transformar os dados da contribuicao em informações para pagamento no checkout
		public override CheckoutCompra transformar(object objetoOrigem) {

			AssociadoContribuicao OAssociadoContribuicao = (AssociadoContribuicao) objetoOrigem;

			CheckoutCompra OCompra = new CheckoutCompra();

			OCompra.flagEntrega = false;

			OCompra.flagRecibo = true;

			OCompra.descricaoDadosCompra = "Dados do Associado";

			OCompra.Pessoa = OAssociadoContribuicao.Associado.Pessoa;

			OCompra.idPessoa = OCompra.Pessoa.id;

			OCompra.nome = OCompra.Pessoa.nome;

			OCompra.nroDocumento = OCompra.Pessoa.nroDocumento;

			OCompra.email = OCompra.Pessoa.emailPrincipal();

			OCompra.telPrincipal = String.Concat(OCompra.Pessoa.dddTelPrincipal, OCompra.Pessoa.nroTelPrincipal);

			OCompra.telSecundario = String.Concat(OCompra.Pessoa.dddTelSecundario, OCompra.Pessoa.nroTelSecundario);

			OCompra.observacaoComprador = String.Format("Tipo de Associado: {0}", OAssociadoContribuicao.Associado.TipoAssociado.descricao);

			OCompra.listaItens = this.carregarProdutos(OAssociadoContribuicao);

			OCompra.valorItens = OCompra.valorTotalProdutos();

			OCompra.TituloReceita = this.carregarTitulo(OAssociadoContribuicao);

			OCompra.idTituloReceita = OCompra.TituloReceita.id;

			return OCompra;
		}

		//Carregar os itens da compra (Mensalidade)
		private List<CheckoutItem> carregarProdutos(AssociadoContribuicao OAssociadoContribuicao){
			
			List<CheckoutItem> listaItens = new List<CheckoutItem>();

			CheckoutItem OItemCompra = new CheckoutItem();
			
			OItemCompra.flagEntrega = false;

			OItemCompra.idTipoItem = TipoItemConst.MENSALIDADE;

			OItemCompra.idItem = OAssociadoContribuicao.id;

			OItemCompra.nomeItem = OAssociadoContribuicao.Contribuicao.descricao;
			
			OItemCompra.descricaoItem = String.Format("Pagamento de Mensalidade referente ao perído de {0}", OAssociadoContribuicao.Contribuicao.anoInicioVigencia.ToString());
			
			OItemCompra.qtdeItem = 1;
			
			OItemCompra.valorUnitario = OAssociadoContribuicao.valorAtual;
			
			OItemCompra.pathImagem = "";
			
			listaItens.Add(OItemCompra);

			return listaItens;
		}

		//Carregar o titulo de receita (se houver)
		private TituloReceita carregarTitulo(AssociadoContribuicao OAssociadoContribuicao) {

			TituloReceita TituloExistente = this.OTituloReceitaMensalidadeBL.carregarPorReceita(OAssociadoContribuicao.id);

			if (TituloExistente != null) {
				return TituloExistente;
			}

			//this.OTituloReceitaMensalidadeBL.gerar( OAssociadoContribuicao as object );

			TituloExistente = this.OTituloReceitaMensalidadeBL.carregarPorReceita(OAssociadoContribuicao.id);

			return TituloExistente;
		}
		
        public override void salvarFrete(CheckoutCompra OCheckoutCompra) { 
			throw new NotImplementedException();
		}

	}
}
