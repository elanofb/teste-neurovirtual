using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ConfiguracoesEcommerce;
using BLL.Financeiro;
using DAL.AssociadosContribuicoes;
using DAL.Contribuicoes;
using DAL.Financeiro;
using DAL.Financeiro.Entities;
using DAL.Pedidos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;

namespace BLL.Pedidos {

	public class TituloReceitaGeradorPedidoBL : TituloReceitaGeradorBL {

		//Atributos
        private ITituloReceitaBL _TituloReceitaBL;
        private IConfiguracaoEcommerceBL _ConfiguracaoEcommerceBL;

		//Propriedades
        private int idTipoReceita { get; set; }
        private ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPedidoBL();
        private IConfiguracaoEcommerceBL OConfiguracaoEcommerceBL => this._ConfiguracaoEcommerceBL = this._ConfiguracaoEcommerceBL ?? new ConfiguracaoEcommerceBL();

        /// <summary>
        /// Construtor
        /// </summary>
        public TituloReceitaGeradorPedidoBL() {

            this.idTipoReceita = TipoReceitaConst.PEDIDO;

        }

		//Metodo para geracao do titulo de receita
		public override UtilRetorno gerar(object OrigemTitulo){

		    Pedido OPedido = (OrigemTitulo as Pedido);

		    if (OPedido == null) {
		        return UtilRetorno.newInstance(true, "O registro Pedido está nulo.");
		    }

		    //Verificar se o titulo já existe
		    var OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(OPedido.id);

		    if (OTituloReceita != null) {

		        return UtilRetorno.newInstance(false, "O título já foi gerado anteriormente.", OTituloReceita);

		    }

			var OConfigEcommerce = OConfiguracaoEcommerceBL.carregar(User.idOrganizacao(), false);
            
		    OTituloReceita = new TituloReceita();

		    OTituloReceita.idPessoa = OPedido.idPessoa;

		    OTituloReceita.idTipoReceita = (byte)idTipoReceita;

		    OTituloReceita.idReceita = OPedido.id;

		    OTituloReceita.idOrganizacao = OPedido.idOrganizacao.toInt();

		    OTituloReceita.idUnidade = OPedido.idUnidade;

		    OTituloReceita.limiteParcelamento = OConfigEcommerce.qtdeLimiteParcelas;
			
		    OTituloReceita.qtdeRepeticao = 1;

		    OTituloReceita.mesCompetencia = (byte?)OPedido.dtFaturamento?.Month;

		    OTituloReceita.anoCompetencia = (short?)OPedido.dtFaturamento?.Year;

		    if (OTituloReceita.mesCompetencia > 0 && OTituloReceita.anoCompetencia > 0){

		        byte? diaCompetencia = OPedido.dtFaturamento?.Day.toByte();

		        diaCompetencia = diaCompetencia.toByte() > 0 ? diaCompetencia.toByte() : (byte)1;

		        OTituloReceita.dtCompetencia = new DateTime(OTituloReceita.anoCompetencia.toInt(), OTituloReceita.mesCompetencia.toInt(), diaCompetencia.toInt());

		    }

		    OTituloReceita.idContaBancaria = OPedido.idContaBancaria;

		    OTituloReceita.idCentroCusto = OPedido.idCentroCusto;

		    OTituloReceita.idMacroConta = OPedido.idMacroConta;

		    OTituloReceita.idCategoria = OPedido.idCategoriaTitulo;

		    OTituloReceita.flagCartaoCreditoPermitido = OPedido.flagCartaoCreditoPermitido;

		    OTituloReceita.flagBoletoBancarioPermitido = OPedido.flagBoletoBancarioPermitido;

		    OTituloReceita.flagDepositoPermitido = OPedido.flagDepositoPermitido;

		    OTituloReceita.descricao = $"Pedido {OPedido.id}";
            
		    OTituloReceita.valorTotal = OPedido.getValorTotal();

		    OTituloReceita.dtVencimentoOriginal = OPedido.dtVencimento;

		    OTituloReceita.dtVencimento = OPedido.dtVencimento;

		    this.preencherRecibo(ref OTituloReceita, OPedido.Pessoa);
            
		    this.salvar(OTituloReceita);

		    return UtilRetorno.newInstance(false, "O título foi gerado com sucesso.", OTituloReceita);

		}

        // Gera títulos em lote
        public override UtilRetorno gerarLote(object OrigemTitulo) {
            throw new NotImplementedException();
        }

    }
}