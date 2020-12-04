using FluentValidation.Attributes;
using DAL.DocumentosDigitais;

namespace WEB.Areas.DocumentosDigitais.ViewModels {

    [Validator(typeof(DocumentoDigitalValidator))]
    public class DocumentoDigitalForm {

		public DocumentoDigital DocumentoDigital { get; set;}

		public DocumentoDigitalForm() {
		}
    }
}