using FluentValidation;

namespace WEB.Areas.Institucionais.ViewModels {

    //
    public class AssociacaoHistoriaFormValidation : AbstractValidator<AssociacaoHistoriaForm> {
        
        //
        public AssociacaoHistoriaFormValidation() {
            RuleFor(x => x.AssociacaoHistoria.titulo)
                .NotEmpty().WithMessage("Informe o título");

            RuleFor(x => x.AssociacaoHistoria.ativo)
                .NotEmpty().WithMessage("Informe o status");

            RuleFor(x => x.AssociacaoHistoria.conteudo)
                .NotEmpty().WithMessage("Insira o conteúdo");
        }
    }
}