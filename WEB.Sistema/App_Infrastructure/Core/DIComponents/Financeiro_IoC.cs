using BLL.Core.Events;
using BLL.Financeiro;
using BLL.Financeiro.Events;
using SimpleInjector;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class Financeiro_IoC {
        
        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {

           
            container.Register<ITituloReceitaPagamentoRejeicaoBL, TituloReceitaPagamentoRejeicaoBL>();
            
            container.Register<ITituloReceitaPagamentoBaixaBL, TituloReceitaPagamentoBaixaBL>();
            
            container.Register<ITituloReceitaPagamentoCadastroBL, TituloReceitaPagamentoCadastroBL>();
            
            container.Register<ITituloReceitaPagamentoConsultaBL, TituloReceitaPagamentoConsultaBL>();
            
            container.Register<IDescontoAntecipacaoConsultaBL, DescontoAntecipacaoConsultaBL>();

            
            container.Register<ITituloDespesaPagamentoCadastroBL, TituloDespesaPagamentoCadastroBL>();
            
            container.RegisterConditional<EventAggregator, OnPagamentoRecebido>(c => c.Consumer.ImplementationType == typeof(TituloReceitaPagamentoBaixaBL));
            
            
        }          
    }

}
