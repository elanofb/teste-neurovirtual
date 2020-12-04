using FluentValidation;

namespace WEB.Areas.Paginas.ViewModels {

    //
    public class PaginaEstatutoFormValidator : AbstractValidator<PaginaEstatutoForm> {
        
        //
        public PaginaEstatutoFormValidator() {

            RuleFor(x => x.PaginaEstatuto.titulo)
                .NotEmpty().WithMessage("Informe o título da página.");
            
            RuleFor(x => x.PaginaEstatuto.texto)
                .NotEmpty().WithMessage("Insira o conteúdo da página.")
                .Length(1, 5000).WithMessage("O conteúdo da página não pode ter mais do que 5000 caracteres (incluindo códigos html).");

        }
    }
}