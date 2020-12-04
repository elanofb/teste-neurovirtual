using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoRepresentanteFormValidator))]
	public class AssociadoRepresentanteForm {

		//Propriedades
		public AssociadoRepresentante AssociadoRepresentante { get; set; }

		//Construtor
		public AssociadoRepresentanteForm() { 
		}

	}
}