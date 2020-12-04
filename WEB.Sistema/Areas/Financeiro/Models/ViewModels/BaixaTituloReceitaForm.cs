using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels {
    // Validação da entidade titulo Despesa pagamento 
    [Validator(typeof(BaixaTituloReceitaFormValidator))]
    public class BaixaTituloReceitaForm : BaixaPagamentoAbstract {

        public TituloReceita TituloReceita { get; set; }

        public override TituloReceitaPagamento TituloReceitaPagamento { get; set; }


        public BaixaTituloReceitaForm() {

            this.TituloReceita = new TituloReceita();

            this.TituloReceitaPagamento = new TituloReceitaPagamento();
        }
    }
}