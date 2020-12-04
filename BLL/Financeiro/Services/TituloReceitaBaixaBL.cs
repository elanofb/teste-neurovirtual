using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Financeiro.Events;
using DAL.Financeiro;
using BLL.Services;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaBaixaBL : DefaultBL, ITituloReceitaBaixaBL {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        protected virtual ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPadraoBL();


        //Eventos
        protected readonly EventAggregator onTituloQuitado = new OnTituloQuitado();


        //Construtor
        public TituloReceitaBaixaBL() {
        }

        //Registrar a liquidacao do titulo (pagamento total)
        //Cada classe filha pode sobreescrever esse metodo para que as acoes de pagamento sejam feitas
        public virtual TituloReceita liquidar(int idReceita, List<TituloReceitaPagamento> listaPagamentos, int idUsuarioBaixa) {
            throw new NotImplementedException();
        }


        //Liquidar receita de acordo com lista de pagamentos informada
        public virtual TituloReceita liquidar(TituloReceita OTituloReceita, List<TituloReceitaPagamento> listaPagamentos) {

            if (OTituloReceita.dtQuitacao.HasValue){

                this.liquidar(OTituloReceita);

                return OTituloReceita;
            }

            //this.OTituloReceitaBL.removerParcelas(OTituloReceita.id, OTituloReceita.idUsuarioAlteracao);

            foreach (var OPagamento in listaPagamentos) {

                OPagamento.idMeioPagamento = OPagamento.definirMeioPagamento();

                OPagamento.idFormaPagamento = OPagamento.definirFormaPagamento();

                OPagamento.dtPrevisaoCredito = OPagamento.dtPrevisaoCredito.HasValue? OPagamento.dtPrevisaoCredito: OPagamento.dtCredito;

                OPagamento.dtCredito = OPagamento.dtCredito;

                OPagamento.idTituloReceita = OTituloReceita.id;

                OPagamento.valorOriginal = UtilNumber.toDecimal(OTituloReceita.valorTotal);

                OPagamento.dtBaixa = DateTime.Now;

                OPagamento.flagBaixaAutomatica = false;

                OPagamento.idUsuarioBaixa = OTituloReceita.idUsuarioAlteracao;

                if (OTituloReceita.dtVencimento.HasValue) {

                    OPagamento.dtVencimento = OTituloReceita.dtVencimento.Value;

                }

                OPagamento.setDefaultInsertValues();

                this.db.TituloReceitaPagamento.Add(OPagamento);
            }

            this.db.SaveChanges();

            this.liquidar(OTituloReceita);

            return OTituloReceita;
        }

        //Liquidar um título de receita a partir do próprio título
        //A parcelas precisam já ter tido os seus pagamentos registrados
        public virtual TituloReceita liquidar(TituloReceita OTituloReceita) {

            var listaParcelas = this.db.TituloReceitaPagamento
                                                .Where(x => x.idTituloReceita == OTituloReceita.id && x.dtExclusao == null)
                                                .ToList();

            decimal valorTotalRecebido = listaParcelas.Where(x => x.dtPagamento.HasValue).Sum(x => x.valorRecebido.toDecimal());

            decimal valorDescontoAntecipacao = listaParcelas.Where(x => x.dtPagamento.HasValue).Sum(x => x.valorDescontoAntecipacao.toDecimal());

            decimal valorDescontoCupom = listaParcelas.Where(x => x.dtPagamento.HasValue).Sum(x => x.valorDescontoCupom.toDecimal());

            decimal valorTotalDescontoParcelas = decimal.Add(valorDescontoCupom, valorDescontoAntecipacao);

            //Diminuir os descontos registros diretamente no título
            decimal valorTotalTitulo = decimal.Subtract(OTituloReceita.valorTotal.toDecimal(), OTituloReceita.valorDesconto.toDecimal());

            //Diminuir os descontos acrescentados no checkout ou por antecipacao de pagamento
            valorTotalTitulo = decimal.Subtract(valorTotalTitulo, valorTotalDescontoParcelas);

            if (valorTotalRecebido < valorTotalTitulo) {

                return OTituloReceita;
            }

            OTituloReceita.dtQuitacao = listaParcelas.Max(x => x.dtPagamento);

            this.db.TituloReceita.Where(x => x.id == OTituloReceita.id)
                                 .Update(x => new TituloReceita {
                                     dtQuitacao = OTituloReceita.dtQuitacao,
                                     ativo = true
                                 }
                                );

            this.onTituloQuitado.publish((OTituloReceita.id as object));

            return OTituloReceita;
        }
    }
}