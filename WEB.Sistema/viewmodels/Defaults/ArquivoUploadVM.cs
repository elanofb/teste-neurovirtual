using System;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Attributes;
using UTIL.Resources;

namespace WEB.ViewModels{

    [Validator(typeof(ArquivoUploadValidator))]
    public class ArquivoUploadVM {

        [HiddenInput]
        public int id { get; set; }

        public string titulo { get; set; }
        public string legenda { get; set; }
        public string categoria { get; set; }
        public string entidade { get; set; }
        public int idReferenciaEntidade { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
    }

    internal class ArquivoUploadValidator : AbstractValidator<ArquivoUploadVM> {
        
        //
        public  ArquivoUploadValidator() {

            RuleFor(x => x.FileUpload)
                .NotEmpty().WithMessage(String.Format(ValidationMessages.Required, "arquivo"));
            
            //RuleFor(x => x.FileUpload)
            //    .Must( (FileUpload) => UTIL.Upload.UploadConfig.validarImagem(FileUpload))
            //    .When( x => x.FileUpload != null && x.categoria != "documento").WithMessage("Imagem inválida");
  
        }
        
    }
}