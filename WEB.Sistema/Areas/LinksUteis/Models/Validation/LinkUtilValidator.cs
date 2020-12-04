using FluentValidation;

namespace WEB.Areas.LinksUteis.ViewModels {
    //
    public class LinkUtilValidator : AbstractValidator<LinkUtilForm> {

        public LinkUtilValidator() {

            RuleFor(x => x.LinkUtil.link)
                .NotEmpty().WithMessage("Informe um link.");

            RuleFor(x => x.LinkUtil.flagBlank)
                .NotEmpty().WithMessage("Abrir em uma nova janela?");

            RuleFor(x => x.LinkUtil.descricao)
                .NotEmpty().WithMessage("Informe uma descrição.");
        }
    }
}