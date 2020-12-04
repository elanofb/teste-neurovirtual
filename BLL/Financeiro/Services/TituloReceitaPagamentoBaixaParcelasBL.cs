using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoBaixaParcelasBL : DefaultBL, ITituloReceitaPagamentoBaixaParcelasBL{

        //Atributos

        //Propriedades

        //Eventos
        
        /// <summary>
        /// Registrar o pagamento das parcelas adicionais de um titulo, a partir da parcela principal
        /// </summary>
        public UtilRetorno registrarPagamento(TituloReceitaPagamento OTituloReceitaPagamento){

	        var listaParcelas = db.TituloReceitaPagamento.Where(x => x.idParcelaPrincipal == OTituloReceitaPagamento.id).ToList();

            if (!listaParcelas.Any()) {
                return UtilRetorno.newInstance(true, "Pagamento não localizado no sistema.");
            }

	        var qtdeParcelas = OTituloReceitaPagamento.qtdeParcelas.toByte();
	        
	        var valorRecebidoParcela = decimal.Divide(OTituloReceitaPagamento.valorRecebido.toDecimal(), qtdeParcelas.toDecimal());

	        foreach (var OParcela in listaParcelas){

		        db.TituloReceitaPagamento
			        	.Where(x => x.id == OParcela.id)
			        	.Update(x => new TituloReceitaPagamento{
				        	idUsuarioBaixa = OTituloReceitaPagamento.idUsuarioBaixa,
				        	idUsuarioAlteracao = OTituloReceitaPagamento.idUsuarioAlteracao,
				        	dtBaixa = OTituloReceitaPagamento.dtBaixa,
					        flagBaixaAutomatica = OTituloReceitaPagamento.flagBaixaAutomatica,
				        	idStatusPagamento = OTituloReceitaPagamento.idStatusPagamento,
					        dtPagamento = OTituloReceitaPagamento.dtPagamento,
					        valorRecebido = valorRecebidoParcela,
				        	codigoAutorizacao = OTituloReceitaPagamento.codigoAutorizacao,
				        	tid = OTituloReceitaPagamento.tid
			        	});
	        }
	        
	        //Ajustar o valor recebido da parcela principal
	        db.TituloReceitaPagamento.Where(x => x.id == OTituloReceitaPagamento.id)
									.Update(x => new TituloReceitaPagamento{
										valorRecebido = valorRecebidoParcela
									});	        
	        
	        

/*            var ODescontoAntecipacao = dbPagamento.TituloReceita.retornarDescontosAntecipacao(OTituloReceitaPagamento.dtPagamento.GetValueOrDefault()).FirstOrDefault();

            if (ODescontoAntecipacao != null){

                dbPagamento.idDescontoAntecipacao = ODescontoAntecipacao.id;

                dbPagamento.valorDescontoAntecipacao = ODescontoAntecipacao.valor;

            }

            db.SaveChanges();

            this.onPagamentoRecebido.subscribe(new OnPagamentoRecebidoHandler());

            this.onPagamentoRecebido.publish( dbPagamento as object);*/
            
            return UtilRetorno.newInstance(false, "O pagamento foi registrado com sucesso.");
        }


    }
}
