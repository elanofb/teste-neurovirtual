using DAL.Representantes;
using FluentValidation.Attributes;

namespace WEB.Areas.Representantes.ViewModels {

	[Validator(typeof(RepresentanteValidator))]
	public class RepresentanteForm {				
		
		//Propriedades
		public Representante Representante { get; set; }
		
		//Construtor
        public RepresentanteForm() {
	        
		}
		
	}
	
}