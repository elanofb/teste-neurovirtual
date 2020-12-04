using BLL.AssociadosContribuicoes;
using BLL.Pedidos;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class TituloReceitaBaixaFactoryBL {

        //Atributos
        private static TituloReceitaBaixaFactoryBL _instance;

        //Propriedades
        public static TituloReceitaBaixaFactoryBL getInstance => _instance = _instance ?? new TituloReceitaBaixaFactoryBL();

        //eventos

        //Definir qual título receita irá fazer os tratamentos
        public ITituloReceitaBaixaBL factory(TituloReceita OTitulo) {

            if(OTitulo == null) {
                return new TituloReceitaBaixaBL();
            }

            if(OTitulo.idTipoReceita == TipoReceitaConst.CONTRIBUICAO) {
                return new TituloReceitaContribuicaoBaixaBL();
            }

            if (OTitulo.idTipoReceita == TipoReceitaConst.PEDIDO) {
                return new TituloReceitaPedidoBaixaBL();
            }

            /*if (OTitulo.idTipoReceita == TipoReceitaConst.INSCRICAO_EVENTO) {
                return new TituloReceitaEventoBaixaBL();
            }

            if (OTitulo.idTipoReceita == TipoReceitaConst.SOLICITACAO) {
                return new TituloReceitaSolicitacaoBaixaBL();
            }

            if (OTitulo.idTipoReceita == TipoReceitaConst.PROCESSO_AVALIACAO) {
                return new TituloReceitaInscricaoProcessoBaixaBL();
            }*/

            return new TituloReceitaBaixaBL();
        }


    }
}