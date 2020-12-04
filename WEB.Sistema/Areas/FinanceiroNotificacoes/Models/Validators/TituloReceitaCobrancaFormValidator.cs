using FluentValidation;

namespace WEB.Areas.FinanceiroNotificacoes.ViewModels {

    //
    public class TituloReceitaCobrancaFormValidator : AbstractValidator<TituloReceitaCobrancaForm> {
        
        //
        public TituloReceitaCobrancaFormValidator() {

            RuleFor(x => x.emailCobrancaTitulo)
                .NotEmpty().WithMessage("Informe o título do e-mail de cobrança.")
                .Length(1, 255).WithMessage("O título do e-mail de cobrança deve ter entre 1 e 255 caracteres.");

            RuleFor(x => x.emailCobrancaHtml)
                .NotEmpty().WithMessage("Informe o corpo do e-mail de cobrança.")
                .Length(1, 3000).WithMessage("O corpo do e-mail de cobrança deve ter entre 1 e 3000 caracteres.");


        }
        
    }
}