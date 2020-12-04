using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {
    
    //
    public class IniciacaoCientificaFormValidation : AbstractValidator<IniciacaoCientificaForm> {
        //
        public IniciacaoCientificaFormValidation() {

            RuleFor(x => x.ONoticia.titulo)
                .NotEmpty().WithMessage("Informe o título da notícia");

            RuleFor(x => x.ONoticia.autor)
                .NotEmpty().WithMessage("Informe o autor da notícia");

            RuleFor(x => x.ONoticia.dtNoticia)
                .NotEmpty().WithMessage("Informe a data da notícia");

            RuleFor(x => x.ONoticia.ativo)
                .NotEmpty().WithMessage("Informe o status");

            RuleFor(x => x.ONoticia.descricao)
                .NotEmpty().WithMessage("Insira a descrição da notícia");

        }
    }
}