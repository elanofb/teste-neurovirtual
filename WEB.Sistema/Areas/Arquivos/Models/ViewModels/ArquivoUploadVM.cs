using System;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;
using UTIL.Resources;

namespace WEB.Areas.Arquivos.ViewModels {

    [Validator(typeof(ArquivoUploadValidator))]
    public class ArquivoUploadVM {

        public int id { get; set; }

        public string titulo { get; set; }

        public string legenda { get; set; }

        public string categoria { get; set; }

        public string entidade { get; set; }

        public int idReferenciaEntidade { get; set; }

        public string urlRedirect { get; set;}

        public bool? flagView { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
    }

	#region Validation

	internal class ArquivoUploadValidator : AbstractValidator<ArquivoUploadVM> {
        
        //
        public  ArquivoUploadValidator() {

            RuleFor(x => x.FileUpload)
                .NotEmpty().WithMessage(String.Format(ValidationMessages.Required, "arquivo"));
            
            //RuleFor(x => x.FileUpload)
            //    .Must( (FileUpload) => UTIL.Upload.UploadConfig.validarImagem(FileUpload))
            //    .When( x => x.FileUpload != null && (x.categoria == ArquivoUploadTypes.LOGOTIPO || x.categoria == ArquivoUploadTypes.FOTO) ).WithMessage("Imagem inválida");
  
        }
        
    }

	#endregion

}