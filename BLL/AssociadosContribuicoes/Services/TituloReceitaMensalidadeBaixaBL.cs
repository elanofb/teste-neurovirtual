using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosContribuicoes.Events;
using BLL.Core.Events;
using BLL.Financeiro;
using BLL.Financeiro.Events;
using DAL.Financeiro;

namespace BLL.AssociadosContribuicoes {

    public class TituloReceitaMensalidadeBaixaBL : TituloReceitaBaixaBL {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;

        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        //Propriedades
        protected override ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaMensalidadeBL();

        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoMensalidadeBL();

        //Eventos
        readonly EventAggregator onMensalidadeQuitada = OnTituloQuitado.getInstance;


        //Construtor
        public TituloReceitaMensalidadeBaixaBL() {
        }


        //Baixar um título com base no id de referencia
        public override TituloReceita liquidar(int idReceita, List<TituloReceitaPagamento> listaPagamentos, int idUsuarioBaixa) {

            var OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(idReceita);

            if(OTituloReceita == null) {

                var OAssociadoMensalidade = this.OAssociadoContribuicaoBL.carregar(idReceita);

                //this.OTituloReceitaBL.gerar(OAssociadoMensalidade);

                OTituloReceita = OTituloReceitaBL.carregarPorReceita(idReceita);
            }

            if(OTituloReceita.dtQuitacao.HasValue) {
                return OTituloReceita;
            }

            //OTituloReceitaBL.removerParcelas(OTituloReceita.id, idUsuarioBaixa);

            foreach(var OPagamento in listaPagamentos) {

                OPagamento.idMeioPagamento = OPagamento.definirMeioPagamento();

                OPagamento.idFormaPagamento = OPagamento.definirFormaPagamento();

                OPagamento.dtCredito = OPagamento.dtCredito;

                OPagamento.idTituloReceita = OTituloReceita.id;

                OPagamento.valorOriginal = OTituloReceita.valorTotal.Value;

                OPagamento.valorRecebido = OTituloReceita.valorTotal.Value;

                OPagamento.dtBaixa = DateTime.Today;

                OPagamento.flagBaixaAutomatica = false;

                OPagamento.idUsuarioBaixa = idUsuarioBaixa;

                if(OTituloReceita.dtVencimento.HasValue) {

                    OPagamento.dtVencimento = OTituloReceita.dtVencimento.Value;

                }

                OPagamento.setDefaultInsertValues();

                this.db.TituloReceitaPagamento.Add(OPagamento);
            }

            this.db.SaveChanges();

            this.liquidar(OTituloReceita);

            this.onMensalidadeQuitada.subscribe(new OnMensalidadeQuitadaHandler());

            this.onMensalidadeQuitada.publish((OTituloReceita.id as object));

            return OTituloReceita;
        }

        //Registrar a liquidacao do titulo (pagamento total)
        public override TituloReceita liquidar(TituloReceita OTituloReceita) {

            var dbTitulo = OTituloReceitaBL.carregar(OTituloReceita.id);

            var listaPagamentos = dbTitulo.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).ToList();

            bool flagTituloQuitado = listaPagamentos.All(x => x.dtPagamento != null);

            if(flagTituloQuitado) {

                dbTitulo.dtQuitacao = listaPagamentos.Max(x => x.dtPagamento);

                db.SaveChanges();

                this.onMensalidadeQuitada.subscribe(new OnMensalidadeQuitadaHandler());

                this.onMensalidadeQuitada.publish((dbTitulo.id as object));
            }

            return OTituloReceita;
        }



    }
}