using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.Core.Events;
using BLL.Financeiro.Events;
using DAL.Financeiro;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoBaixaBL : ITituloReceitaPagamentoBaixaBL {
	    
        //Servicos
	    private ITituloReceitaPagamentoConsultaBL PagamentoConsultaBL { get; }
		private ITituloReceitaPagamentoCadastroBL PagamentoCadastroBL { get; }
	    private IDescontoAntecipacaoConsultaBL AntecipacaoConsultaBL { get; }
	    private IPrincipal User;
		
		//Eventos
		private EventAggregator onPagamentoRecebido { get; }
		
	    /// <summary>
	    /// Construtor
	    /// </summary>
	    public TituloReceitaPagamentoBaixaBL(ITituloReceitaPagamentoConsultaBL _PagamentoConsultaBL,
											ITituloReceitaPagamentoCadastroBL _PagamentoCadastroBL,
											IDescontoAntecipacaoConsultaBL _AntecipacaoConsultaBL,
											EventAggregator _pagamentoRecebido) {

		    PagamentoConsultaBL = _PagamentoConsultaBL;

			PagamentoCadastroBL = _PagamentoCadastroBL;

			AntecipacaoConsultaBL = _AntecipacaoConsultaBL;

			onPagamentoRecebido = _pagamentoRecebido;

		    User = HttpContextFactory.Current.User;
	    }

		/// <summary>
		/// Registrar o pagamento de uma parcela de um titullo
		/// </summary>
		public UtilRetorno registrarPagamento(int idTituloReceitaPagamento, DateTime dtPagamentoPrm) {

			TituloReceitaPagamento OTituloReceitaPagamento = this.carregarDadosPagamento(new TituloReceitaPagamento{ id = idTituloReceitaPagamento });

			if (OTituloReceitaPagamento == null) {
				return UtilRetorno.newInstance(true, "O pagamento informado não pôde ser localizado.");
			}

			if (OTituloReceitaPagamento.dtPagamento.HasValue) {
				return UtilRetorno.newInstance(true, "O pagamento informado já está quitado.");
			}

			this.PagamentoCadastroBL.db
								.TituloReceitaPagamento
								.Where(x => x.id == idTituloReceitaPagamento && x.dtPagamento == null)
								.Update(x => new TituloReceitaPagamento {
																			dtPagamento = dtPagamentoPrm,
																			dtBaixa = DateTime.Now,
																			idUsuarioBaixa = User.id(),
																			flagBaixaAutomatica = false,
																			idUsuarioAlteracao = User.id(),
																			valorRecebido = OTituloReceitaPagamento.valorOriginal
																		});

			return UtilRetorno.newInstance(false, "O pagamento foi registrado com sucesso.");
		}
        
        /// <summary>
        /// Registrar o pagamento de uma parcela de um titulo
        /// Utilizado para a realizacao das baixas manuais 
        /// </summary>
        public UtilRetorno registrarPagamento(TituloReceitaPagamento OTituloReceitaPagamento) {

			OTituloReceitaPagamento.idUsuarioBaixa = User.id();

			OTituloReceitaPagamento.idUsuarioAlteracao = User.id();

			OTituloReceitaPagamento.dtBaixa = DateTime.Now;

			OTituloReceitaPagamento.idFormaPagamento = OTituloReceitaPagamento.idFormaPagamento > 0? OTituloReceitaPagamento.idFormaPagamento : OTituloReceitaPagamento.definirFormaPagamento();

			OTituloReceitaPagamento = this.AntecipacaoConsultaBL.carregarDescontoAntecipacao(OTituloReceitaPagamento);
			
			this.PagamentoCadastroBL.atualizarDadosPagamento(OTituloReceitaPagamento);

			if (OTituloReceitaPagamento.qtdeParcelas > 1 && OTituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_CREDITO) {

				this.registrarPagamentoParcelas(OTituloReceitaPagamento);
			}

            this.onPagamentoRecebido.subscribe(new OnPagamentoRecebidoHandler());

            this.onPagamentoRecebido.publish( OTituloReceitaPagamento as object);
            
            return UtilRetorno.newInstance(false, "O pagamento foi registrado com sucesso.");
        }

		/// <summary>
		/// Procura parcelas vinculadas ao pagamento principal e realiza as baixas
		/// </summary>
		public UtilRetorno registrarPagamentoParcelas(TituloReceitaPagamento OTituloReceitaPagamento) {

	        var listaParcelas = this.carregarParcelas(OTituloReceitaPagamento);

            if (!listaParcelas.Any()) {
                return UtilRetorno.newInstance(true, "Não existem parcelas para o título informado.");
            }

			var qtdeParcelas = listaParcelas.Count.toByte();

			var valorTotalRecebido = OTituloReceitaPagamento.valorRecebido.toDecimal();
	        
	        var valorRecebidoParcela = decimal.Divide(valorTotalRecebido, qtdeParcelas.toDecimal());
			
	        foreach (var OParcela in listaParcelas) {

				OParcela.idUsuarioBaixa = OTituloReceitaPagamento.idUsuarioBaixa;
				
				OParcela.idUsuarioAlteracao = OTituloReceitaPagamento.idUsuarioAlteracao;
				
				OParcela.dtBaixa = OTituloReceitaPagamento.dtBaixa;
				
				OParcela.flagBaixaAutomatica = OTituloReceitaPagamento.flagBaixaAutomatica;
				
				OParcela.idStatusPagamento = OTituloReceitaPagamento.idStatusPagamento;
				
				OParcela.dtPagamento = OTituloReceitaPagamento.dtPagamento;
				
				OParcela.valorRecebido = OParcela.valorRecebido;
				
				OParcela.codigoAutorizacao = OTituloReceitaPagamento.codigoAutorizacao;
				
				OParcela.tid = OTituloReceitaPagamento.tid;

				this.PagamentoCadastroBL.atualizarDadosPagamento(OParcela);
			}
	        
	        //Ajustar o valor recebido da parcela principal que inicialmente recebe o valor cheio do recebimento
			this.PagamentoCadastroBL.db
									.TituloReceitaPagamento.Where(x => x.id == OTituloReceitaPagamento.id)
									.Update(x => new TituloReceitaPagamento{
										valorRecebido = valorRecebidoParcela
									});	        
	        
			
            return UtilRetorno.newInstance(false, "O pagamento foi registrado com sucesso.");
			
		}

		/// <summary>
		/// 
		/// </summary>
		private TituloReceitaPagamento carregarDadosPagamento(TituloReceitaPagamento OTituloReceitaPagamento) {
		    
			var dbPagamento = PagamentoConsultaBL.query(0)
											.Where(x => x.id == OTituloReceitaPagamento.id)
											.Select(x => new {
																x.id,
																x.idTituloReceita,
																x.dtPrevisaoCredito,
																x.valorOriginal,
																x.idUsuarioBaixa
															})
											.FirstOrDefault()
											.ToJsonObject<TituloReceitaPagamento>();
			return dbPagamento;
		}
		
		/// <summary>
		/// 
		/// </summary>
		private List<TituloReceitaPagamento> carregarParcelas(TituloReceitaPagamento OTituloReceitaPagamento) {
		    
			var listaParcelas = PagamentoConsultaBL.query(0)
											.Where(x => 
												x.idParcelaPrincipal == OTituloReceitaPagamento.id && 
												x.tokenTransacao == OTituloReceitaPagamento.tokenTransacao
											)
											.Select(x => new {
																x.id,
																x.idTituloReceita,
																x.dtPrevisaoCredito,
																x.valorOriginal,
																x.valorTarifasBancarias,
																x.valorTarifasTransacao,
																x.valorJuros,
																x.idUsuarioBaixa
															})
											.ToListJsonObject<TituloReceitaPagamento>();
			return listaParcelas;
		}		
    }

}
