using System;
using BLL.Financeiro;
using DAL.ContasBancarias;
using DAL.Financeiro;

namespace BLL.ContasBancarias {

	public class TituloDespesaGeradorMovimentacaoBL : TituloDespesaGeradorBL {

		//Atributos
        private ITituloDespesaBL _TituloDespesaBL;
        private IContaBancariaBL _IContaBancariaBL;
	    private ITituloDespesaPagamentoCadastroBL _ITituloDespesaPagamentoCadastroBL;

		//Propriedades
        private int? idTipoDespesa { get; set; }
        private ITituloDespesaBL OTituloDespesaBL => this._TituloDespesaBL = this._TituloDespesaBL ?? new TituloDespesaMovimentacaoBL();
	    private IContaBancariaBL OContaBancariaBL => _IContaBancariaBL = _IContaBancariaBL ?? new ContaBancariaBL();
	    private ITituloDespesaPagamentoCadastroBL OTituloDespesaPagamentoCadastroBL => _ITituloDespesaPagamentoCadastroBL = _ITituloDespesaPagamentoCadastroBL ?? new TituloDespesaPagamentoCadastroBL();

        /// <summary>
        /// Construtor
        /// </summary>
        public TituloDespesaGeradorMovimentacaoBL() {

            this.idTipoDespesa = null;

        }

		//Metodo para geracao do titulo de Despesa
		public override UtilRetorno gerar(object OrigemTitulo){

		    var OContaBancariaMovimentacao = (OrigemTitulo as ContaBancariaMovimentacao);

		    if (OContaBancariaMovimentacao == null) {
		        return UtilRetorno.newInstance(true, "O registro ContaMovimentação está nulo.");
		    }

		    //Verificar se o titulo já existe
		    var OTituloDespesa = this.OTituloDespesaBL.carregarPorDespesa(OContaBancariaMovimentacao.id);

		    if (OTituloDespesa != null) {

		        return UtilRetorno.newInstance(false, "O título já foi gerado anteriormente.", OTituloDespesa);

		    }
            
		    OTituloDespesa = new TituloDespesa();
            
		    OTituloDespesa.idTipoDespesa = idTipoDespesa;

		    OTituloDespesa.idDespesa = OContaBancariaMovimentacao.id;

		    OTituloDespesa.idOrganizacao = OContaBancariaMovimentacao.idOrganizacao.toInt();

		    OTituloDespesa.idUnidade = OContaBancariaMovimentacao.idUnidade;

		    OTituloDespesa.qtdeRepeticao = 1;

		    OTituloDespesa.mesCompetencia = (byte?)OContaBancariaMovimentacao.dtOperacao.Month;

		    OTituloDespesa.anoCompetencia = (short?)OContaBancariaMovimentacao.dtOperacao.Year;

		    if (OTituloDespesa.mesCompetencia > 0 && OTituloDespesa.anoCompetencia > 0){

		        byte? diaCompetencia = OContaBancariaMovimentacao.dtOperacao.Day.toByte();

		        diaCompetencia = diaCompetencia.toByte() > 0 ? diaCompetencia.toByte() : (byte)1;

		        OTituloDespesa.dtDespesa = new DateTime(OTituloDespesa.anoCompetencia.toInt(), OTituloDespesa.mesCompetencia.toInt(), diaCompetencia.toInt());

		    }

		    OTituloDespesa.idContaBancaria = OContaBancariaMovimentacao.idContaBancariaOrigem;
            
            var OContaBancariaDestino = this.OContaBancariaBL.carregar(OContaBancariaMovimentacao.idContaBancariaDestino) ?? new ContaBancaria();

		    OTituloDespesa.descricao = $"Transferência realizada para a conta { OContaBancariaDestino.OBanco?.nome }: { OContaBancariaDestino.nroConta }/{ OContaBancariaDestino.nroAgencia }";
            
		    OTituloDespesa.valorTotal = OContaBancariaMovimentacao.valor;
            
		    OTituloDespesa.dtVencimento = DateTime.Now;

		    OTituloDespesa.dtQuitacao = DateTime.Now;

		    this.salvar(OTituloDespesa);

            if (OTituloDespesa.id > 0) {

                this.gerarPagamento(OTituloDespesa);

            }

		    return UtilRetorno.newInstance(false, "O título foi gerado com sucesso.", OTituloDespesa);

		}

        // Gera títulos em lote
        public override UtilRetorno gerarLote(object OrigemTitulo) {
            throw new NotImplementedException();
        }

        //
        private void gerarPagamento(TituloDespesa OTituloDespesa) {

            var OPagamento = new TituloDespesaPagamento();

            OPagamento.transferirDadosTitulo(OTituloDespesa);

            OPagamento.idMeioPagamento = MeioPagamentoConst.TRANSFERENCIA_ELETRONICA;

            OPagamento.idFormaPagamento = FormaPagamentoConst.TRANSFERENCIA_BANCARIA;

            OPagamento.idStatusPagamento = StatusPagamentoConst.PAGO;

            OPagamento.dtPagamento = OTituloDespesa.dtQuitacao;

            OPagamento.dtDebito = OTituloDespesa.dtQuitacao;

            OPagamento.valorPago = OTituloDespesa.valorTotal; 

            this.OTituloDespesaPagamentoCadastroBL.salvar(OPagamento);

        }

    }
}