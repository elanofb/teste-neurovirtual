using FluentValidation;

namespace WEB.Areas.AssociadosNotificacoes.ViewModels {

    //
    public class AssociadoContribuicaoCobrancaFormValidator : AbstractValidator<AssociadoContribuicaoCobrancaForm> {
        
        //
        public AssociadoContribuicaoCobrancaFormValidator() {

            RuleFor(x => x.Contribuicao.emailCobrancaTitulo)
                .NotEmpty().WithMessage("Informe o título do e-mail de cobrança.")
                .Length(1, 255).WithMessage("O título do e-mail de cobrança deve ter entre 1 e 255 caracteres.");

            RuleFor(x => x.Contribuicao.emailCobrancaHtml)
                .NotEmpty().WithMessage("Informe o corpo do e-mail de cobrança.")
                .Length(1, 3000).WithMessage("O corpo do e-mail de cobrança deve ter entre 1 e 3000 caracteres.");


        }
        
    }
}