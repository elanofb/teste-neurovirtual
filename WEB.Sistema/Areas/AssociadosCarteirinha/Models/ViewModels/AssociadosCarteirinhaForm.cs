using DAL.AssociadosCarteirinha;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosCarteirinha.ViewModels {

    [Validator(typeof(AssociadosCarteirinhaFormValidator))]
	public class AssociadosCarteirinhaForm {

		//Propriedades
		public AssociadoCarteirinha AssociadoCarteirinha { get; set; }

		//Construtor
		public AssociadosCarteirinhaForm() { 
            this.AssociadoCarteirinha = new AssociadoCarteirinha();
		}

	}
}