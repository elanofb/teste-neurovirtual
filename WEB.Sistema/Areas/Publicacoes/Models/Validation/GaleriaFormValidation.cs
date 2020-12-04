using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class GaleriaFormValidation : AbstractValidator<GaleriaFotoForm> {

        //
        public GaleriaFormValidation() {
            RuleFor(x => x.GaleriaFoto.titulo)
                .NotEmpty().WithMessage("Favor preencha o campo título!");

            RuleFor(x => x.GaleriaFoto.descricao)
                .NotEmpty().WithMessage("Favor preencha o campo descrição!");
        }

    }
}