using DAL.Portais;
using FluentValidation.Attributes;

namespace WEB.Areas.Portais.ViewModels{

    [Validator(typeof(PortalFormValidator))]
	public class PortalForm{

        public Portal Portal { get; set; } 

	}
}