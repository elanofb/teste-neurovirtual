using FluentValidation;

namespace WEB.Areas.DocumentosDigitais.ViewModels {
    //
    public class DocumentoDigitalValidator : AbstractValidator<DocumentoDigitalForm> {

        public DocumentoDigitalValidator() {

            RuleFor(x => x.DocumentoDigital.titulo)
                .NotEmpty().WithMessage("Informe um titulo.");

            RuleFor(x => x.DocumentoDigital.idTipoDocumentoDigital)
                .NotEmpty().WithMessage("Informe o tipo do documento digital.");

            RuleFor(x => x.DocumentoDigital.flagTipoPessoa)
                .NotEmpty().WithMessage("Informe o tipo de pessoa.");

            RuleFor(x => x.DocumentoDigital.ativo)
                .NotEmpty().WithMessage("Informe o status do documento digital.");
        }
    }
}