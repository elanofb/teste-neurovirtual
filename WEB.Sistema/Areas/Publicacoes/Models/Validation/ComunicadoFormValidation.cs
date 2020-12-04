using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class ComunicadoFormValidation : AbstractValidator<ComunicadoForm> {
        //
        public ComunicadoFormValidation() {

            RuleFor(x => x.Noticia.titulo)
                .NotEmpty().WithMessage("Informe o titulo do comunicado");

            RuleFor(x => x.Noticia.dtNoticia)
                .NotEmpty().WithMessage("Informe a data do comunicado");

            RuleFor(x => x.Noticia.ativo)
                .NotEmpty().WithMessage("Informe o status");

            RuleFor(x => x.Noticia.descricao)
                .NotEmpty().WithMessage("Insira a descrição da comunicado");
        }
    }
}