using System;
using System.Linq;
using BLL.Core.Events;
using BLL.Financeiro.Events;
using DAL.Financeiro;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoBaixaCheckoutBL : DefaultBL {

        //Atributos

        //Propriedades

        //Eventos
        private readonly EventAggregator eventoPagamentoRecebido = OnPagamentoRecebido.getInstance;
        
        /// <summary>
        /// Registrar o pagamento de uma parcela de um titulo
        /// Utilizado para a realizacao das baixas manuais 
        /// </summary>
        public UtilRetorno registrarPagamento(TituloReceitaPagamento OTituloReceitaPagamento) {

            var dbPagamento = db.TituloReceitaPagamento.FirstOrDefault(x => x.id == OTituloReceitaPagamento.id);

            if (dbPagamento == null) {
                return UtilRetorno.newInstance(true, "Pagamento não localizado no sistema.");
            }

			dbPagamento.idUsuarioBaixa = User.id();

            dbPagamento.idUsuarioAlteracao = User.id();

            dbPagamento.dtBaixa = DateTime.Now;
		   
            dbPagamento.flagBaixaAutomatica = false;

			dbPagamento.idStatusPagamento = StatusPagamentoConst.PAGO;

            dbPagamento.idMeioPagamento = OTituloReceitaPagamento.idMeioPagamento;

		    dbPagamento.idFormaPagamento = OTituloReceitaPagamento.definirFormaPagamento();

			dbPagamento.dtPagamento = OTituloReceitaPagamento.dtPagamento;

		    dbPagamento.dtCredito = OTituloReceitaPagamento.dtCredito;

            dbPagamento.valorJuros = OTituloReceitaPagamento.valorJuros;

            dbPagamento.valorTarifasBancarias = OTituloReceitaPagamento.valorTarifasBancarias;

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.codigoAutorizacao)) {
    		    dbPagamento.codigoAutorizacao = OTituloReceitaPagamento.codigoAutorizacao;
		    }

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.nroBanco)) {
                dbPagamento.nroBanco = OTituloReceitaPagamento.nroBanco;
		    }

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.nroDocumento)) {
                dbPagamento.nroDocumento = OTituloReceitaPagamento.nroDocumento;
		    }

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.nroAgencia)) {
                dbPagamento.nroAgencia = OTituloReceitaPagamento.nroAgencia;
		    }

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.nroDigitoAgencia)) {
                dbPagamento.nroDigitoAgencia = OTituloReceitaPagamento.nroDigitoAgencia;
		    }

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.nroConta)) {
                dbPagamento.nroConta = OTituloReceitaPagamento.nroConta;
		    }

		    if (!String.IsNullOrEmpty(OTituloReceitaPagamento.nroDigitoConta)) {
                dbPagamento.nroDigitoConta = OTituloReceitaPagamento.nroDigitoConta;
		    }

            if (!(OTituloReceitaPagamento.valorRecebido > 0)) {

                dbPagamento.valorRecebido = dbPagamento.valorOriginal;
            }

            db.SaveChanges();

            this.eventoPagamentoRecebido.subscribe(new OnPagamentoRecebidoHandler());

            this.eventoPagamentoRecebido.publish( dbPagamento as object);
            
            return UtilRetorno.newInstance(false, "O pagamento foi registrado com sucesso.");
        }


        //Registrar o pagamento de uma parcela de um titullo
        public UtilRetorno registrarPagamento(int idTituloReceitaPagamento, DateTime dtPagamento) {

            TituloReceitaPagamento OTituloReceitaPagamento = db.TituloReceitaPagamento.FirstOrDefault(x => x.id == idTituloReceitaPagamento);

            if (OTituloReceitaPagamento == null) {
                return UtilRetorno.newInstance(true, "O pagamento informado não pôde ser localizado.");
            }

            if (OTituloReceitaPagamento.dtPagamento.HasValue) {
                return UtilRetorno.newInstance(true, "O pagamento informado já está quitado.");
            }

            OTituloReceitaPagamento.dtPagamento = dtPagamento;

            OTituloReceitaPagamento.dtBaixa = DateTime.Now;

            OTituloReceitaPagamento.idUsuarioBaixa = User.id();

            OTituloReceitaPagamento.flagBaixaAutomatica = false;

            OTituloReceitaPagamento.idUsuarioAlteracao = OTituloReceitaPagamento.idUsuarioBaixa;

            OTituloReceitaPagamento.valorRecebido = OTituloReceitaPagamento.valorOriginal;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "O pagamento foi registrado com sucesso.");
        }



    }
}
