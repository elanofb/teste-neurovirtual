using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoAbrangenciaFormValidator))]
	public class AssociadoAbrangenciaForm {

		//Propriedades
		public AssociadoAbrangencia AssociadoAbrangencia { get; set; }

		//Construtor
		public AssociadoAbrangenciaForm() { 
		}

	}
}