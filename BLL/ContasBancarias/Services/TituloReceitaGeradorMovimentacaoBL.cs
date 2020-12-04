using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.AssociadosContribuicoes;
using DAL.ContasBancarias;
using DAL.Contribuicoes;
using DAL.Financeiro;
using DAL.Financeiro.Entities;
using DAL.Pedidos;
using DAL.Pessoas;

namespace BLL.ContasBancarias {

	public class TituloReceitaGeradorMovimentacaoBL : TituloReceitaGeradorBL {

		//Atributos
        private ITituloReceitaBL _TituloReceitaBL;
        private IContaBancariaBL _IContaBancariaBL;
	    private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

		//Propriedades
        private int idTipoReceita { get; set; }
        private ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaMovimentacaoBL();
	    private IContaBancariaBL OContaBancariaBL => _IContaBancariaBL = _IContaBancariaBL ?? new ContaBancariaBL();
	    private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

        /// <summary>
        /// Construtor
        /// </summary>
        public TituloReceitaGeradorMovimentacaoBL() {

            this.idTipoReceita = TipoReceitaConst.TRANSFERENCIA;

        }

		//Metodo para geracao do titulo de receita
		public override UtilRetorno gerar(object OrigemTitulo){

		    var OContaBancariaMovimentacao = (OrigemTitulo as ContaBancariaMovimentacao);

		    if (OContaBancariaMovimentacao == null) {
		        return UtilRetorno.newInstance(true, "O registro ContaMovimentação está nulo.");
		    }

		    //Verificar se o titulo já existe
		    var OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(OContaBancariaMovimentacao.id);

		    if (OTituloReceita != null) {

		        return UtilRetorno.newInstance(false, "O título já foi gerado anteriormente.", OTituloReceita);

		    }
            
		    OTituloReceita = new TituloReceita();
            
		    OTituloReceita.idTipoReceita = (byte)idTipoReceita;

		    OTituloReceita.idReceita = OContaBancariaMovimentacao.id;

		    OTituloReceita.idOrganizacao = OContaBancariaMovimentacao.idOrganizacao.toInt();

		    OTituloReceita.idUnidade = OContaBancariaMovimentacao.idUnidade;

		    OTituloReceita.qtdeRepeticao = 1;

		    OTituloReceita.mesCompetencia = (byte?)OContaBancariaMovimentacao.dtOperacao.Month;

		    OTituloReceita.anoCompetencia = (short?)OContaBancariaMovimentacao.dtOperacao.Year;

		    if (OTituloReceita.mesCompetencia > 0 && OTituloReceita.anoCompetencia > 0){

		        byte? diaCompetencia = OContaBancariaMovimentacao.dtOperacao.Day.toByte();

		        diaCompetencia = diaCompetencia.toByte() > 0 ? diaCompetencia.toByte() : (byte)1;

		        OTituloReceita.dtCompetencia = new DateTime(OTituloReceita.anoCompetencia.toInt(), OTituloReceita.mesCompetencia.toInt(), diaCompetencia.toInt());

		    }

		    OTituloReceita.idContaBancaria = OContaBancariaMovimentacao.idContaBancariaDestino;
            
            var OContaBancariaOrigem = this.OContaBancariaBL.carregar(OContaBancariaMovimentacao.idContaBancariaOrigem) ?? new ContaBancaria();

		    OTituloReceita.descricao = $"Transferência recebida à partir da conta { OContaBancariaOrigem.OBanco?.nome }: { OContaBancariaOrigem.nroConta }/{ OContaBancariaOrigem.nroAgencia }";
            
		    OTituloReceita.valorTotal = OContaBancariaMovimentacao.valor;

		    OTituloReceita.dtVencimentoOriginal = DateTime.Now; 

		    OTituloReceita.dtVencimento = DateTime.Now;

		    OTituloReceita.dtQuitacao = DateTime.Now;
            
		    this.salvar(OTituloReceita);

            if (OTituloReceita.id > 0) {

                this.gerarPagamento(OTituloReceita);

            }

		    return UtilRetorno.newInstance(false, "O título foi gerado com sucesso.", OTituloReceita);

		}

        // Gera títulos em lote
        public override UtilRetorno gerarLote(object OrigemTitulo) {
            throw new NotImplementedException();
        }

	    //
	    private void gerarPagamento(TituloReceita OTituloReceita) {

	        var OPagamento = new TituloReceitaPagamento();

	        OPagamento.transferirDadosTitulo(OTituloReceita);

	        OPagamento.idMeioPagamento = MeioPagamentoConst.TRANSFERENCIA_ELETRONICA;

	        OPagamento.idFormaPagamento = FormaPagamentoConst.TRANSFERENCIA_BANCARIA;

	        OPagamento.idStatusPagamento = StatusPagamentoConst.PAGO;

	        OPagamento.dtPagamento = OTituloReceita.dtQuitacao;

	        OPagamento.dtCredito = OTituloReceita.dtQuitacao;

	        OPagamento.valorRecebido = OTituloReceita.valorTotal;

	        OTituloReceitaPagamentoBL.salvar(OPagamento);

	    }

    }

}