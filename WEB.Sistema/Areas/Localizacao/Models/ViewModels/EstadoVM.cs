using DAL.Localizacao;
using FluentValidation.Attributes;

namespace WEB.Areas.Localizacao.ViewModels {

    [Validator(typeof(EstadoValidator))]
	public class EstadoVM {

		public Estado Estado { get; set;}

	}
    
}