using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class VideoFormValidation : AbstractValidator<VideoForm> {
        //
        public VideoFormValidation() {

            RuleFor(x => x.Video.titulo)
                .NotEmpty().WithMessage("Informe o título");

            RuleFor(x => x.Video.urlVideo)
                .NotEmpty().WithMessage("Favor inserir a url do video!");

            RuleFor(x => x.Video.descricao)
                .NotEmpty().WithMessage("Favor inserir uma breve descrição!");

            RuleFor(x => x.Video.ativo)
                .NotEmpty().WithMessage("Favor selecionar um status!");
        }

    }
}