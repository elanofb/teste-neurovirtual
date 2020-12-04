using DAL.Ajudas;
using FluentValidation.Attributes;

namespace WEB.Areas.Ajudas.ViewModels{

    [Validator(typeof(AjudaModuloFormValidator))]
	public class AjudaModuloForm{

        public AjudaModulo AjudaModulo { get; set; } 

	}
}