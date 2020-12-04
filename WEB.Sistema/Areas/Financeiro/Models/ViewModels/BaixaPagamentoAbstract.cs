using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public abstract class BaixaPagamentoAbstract {
        
        public abstract TituloReceitaPagamento TituloReceitaPagamento { get; set; }
    }

}
