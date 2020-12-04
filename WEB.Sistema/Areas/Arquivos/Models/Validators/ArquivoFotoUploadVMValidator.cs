using System;
using System.Web;
using FluentValidation;
using UTIL.Resources;

namespace WEB.Areas.Arquivos.ViewModels {
    
	public class ArquivoUploadFotoVMValidator : AbstractValidator<ArquivoUploadFotoVM> {
        
        //
        public ArquivoUploadFotoVMValidator() {

            RuleFor(x => x.FileUpload)
                .NotEmpty().WithMessage(String.Format(ValidationMessages.Required, "arquivo"));

            RuleFor(x => x.FileUpload).SetCollectionValidator(new FileUploadValidator());

        }
        
    }
    
    internal class FileUploadValidator : AbstractValidator<HttpPostedFileBase> {

        public FileUploadValidator() {

            RuleFor(x => x)
                .Must((x, FileUpload) => UTIL.Upload.UploadConfig.validarImagem(x))
                .WithMessage("Alguma(s) imagem(ns) não é(são) válida(s).");

        }

    }

}