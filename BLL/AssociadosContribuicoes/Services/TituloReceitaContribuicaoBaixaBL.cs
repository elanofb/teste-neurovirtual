using System.Collections.Generic;
using BLL.AssociadosContribuicoes.Events;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.AssociadosContribuicoes {

    public class TituloReceitaContribuicaoBaixaBL : TituloReceitaBaixaBL {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;
        private ITituloReceitaGeradorBL _TituloReceitaGeradorBL;
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        //Propriedades
        protected override ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaContribuicaoBL();
        private ITituloReceitaGeradorBL OTituloReceitaGeradorBL => this._TituloReceitaGeradorBL = this._TituloReceitaGeradorBL ?? new TituloReceitaGeradorContribuicaoBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();

        //Eventos


        //Construtor
        public TituloReceitaContribuicaoBaixaBL() {
        }

        //Baixar um título com base no id de referencia
        public override TituloReceita liquidar(int idReceita, List<TituloReceitaPagamento> listaPagamentos, int idUsuarioBaixa) {

            var OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(idReceita);

            if (OTituloReceita == null) {

                var OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(idReceita);

                this.OTituloReceitaGeradorBL.gerar(OAssociadoContribuicao);

                OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(idReceita);
            }

            OTituloReceita.idUsuarioAlteracao = idUsuarioBaixa;

            return this.liquidar(OTituloReceita, listaPagamentos);
        }

        //Registrar a liquidacao do titulo (pagamento total)
        public override TituloReceita liquidar(TituloReceita OTituloReceita) {

            
            //;this.onTituloQuitado.subscribe(new OnContribuicaoQuitadaHandler());

            return base.liquidar(OTituloReceita);
        }

    }
}