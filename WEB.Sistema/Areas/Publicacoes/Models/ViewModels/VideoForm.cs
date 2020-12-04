using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(VideoFormValidation))]
    public class VideoForm {

        public Video Video { get; set; }
    }

}