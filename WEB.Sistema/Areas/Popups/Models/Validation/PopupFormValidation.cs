using FluentValidation;

namespace WEB.Areas.Popups.ViewModels {

    //
    public class PopupFormValidation : AbstractValidator<PopupForm> {
        //
        public PopupFormValidation() {
            RuleFor(x => x.OHomePopup.titulo).NotEmpty().WithMessage("Informe o título do popup");
            RuleFor(x => x.OHomePopup.conteudo).NotEmpty().WithMessage("Informe o conteúdo do popup");
            RuleFor(x => x.OHomePopup.dtInicioExibicao).NotEmpty().WithMessage("Informe a data de início de vigência do popup.");
            RuleFor(x => x.OHomePopup.dtFimExibicao).NotEmpty().WithMessage("Informe a data de término de vigência do popup.");
        }
    }
}