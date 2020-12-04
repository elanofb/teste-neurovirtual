using DAL.Localizacao;
using FluentValidation.Attributes;

namespace WEB.Areas.Localizacao.ViewModels {

    [Validator(typeof(PaisValidator))]
	public class PaisVM{

		public Pais Pais { get; set;}

	}
    
}