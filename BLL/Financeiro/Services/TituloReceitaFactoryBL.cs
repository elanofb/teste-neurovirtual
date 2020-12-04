using BLL.Associados;
using BLL.AssociadosContribuicoes;
using BLL.Eventos;
using BLL.Pedidos;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class TituloReceitaFactoryBL {

        //Atributos
        private static TituloReceitaFactoryBL _instance;

        //Propriedades
        public static TituloReceitaFactoryBL getInstance => _instance = _instance ?? new TituloReceitaFactoryBL();

        //eventos

        //Definir qual título receita irá fazer os tratamentos
                 public ITituloReceitaBL factory(TituloReceita OTitulo) {
         
                     if(OTitulo == null) {
                         return new TituloReceitaPadraoBL();
                     }
         
                     if(OTitulo.idTipoReceita == TipoReceitaConst.CONTRIBUICAO) {
                         return new TituloReceitaContribuicaoBL();
                     }
         
         
/*
                     if(OTitulo.idTipoReceita == TipoReceitaConst.INSCRICAO_EVENTO) {
                         return new TituloReceitaEventoBL();
                     }
*/
         
                     if(OTitulo.idTipoReceita == TipoReceitaConst.PEDIDO) {
                         return new TituloReceitaPedidoBL();
                     }
         
                     if(OTitulo.idTipoReceita == TipoReceitaConst.TAXA_INSCRICAO) {
                         return new TituloReceitaTaxaInscricaoBL();
                     }
         
                     return new TituloReceitaPadraoBL();
                 }


    }
}