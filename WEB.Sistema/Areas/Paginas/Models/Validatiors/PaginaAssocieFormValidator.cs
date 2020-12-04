using FluentValidation;

namespace WEB.Areas.Paginas.ViewModels {

    //
    public class PaginaAssocieFormValidator : AbstractValidator<PaginaAssocieForm> {
        
        //
        public PaginaAssocieFormValidator() {

            RuleFor(x => x.PaginaAssocie.titulo)
                .NotEmpty().WithMessage("Informe o título da página.");
            
            RuleFor(x => x.PaginaAssocie.texto)
                //.NotEmpty().WithMessage("Insira o conteúdo da página.")
                .Length(0, 7000).WithMessage("O conteúdo da página não pode ter mais do que 5000 caracteres (incluindo códigos html).");

        }
    }
}