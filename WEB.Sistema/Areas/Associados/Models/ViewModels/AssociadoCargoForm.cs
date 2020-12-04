using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoCargoFormValidator))]
	public class AssociadoCargoForm {

		//Propriedades
		public AssociadoCargo AssociadoCargo { get; set; }

		//Construtor
		public AssociadoCargoForm() { 
		}

	}
}